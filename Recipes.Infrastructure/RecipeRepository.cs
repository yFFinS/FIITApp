using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;

namespace Recipes.Infrastructure;

public class RecipeRepository : IRecipeRepository
{
    private readonly ILogger<RecipeRepository> _logger;
    private readonly IDataBase _dataBase;
    private readonly IProductRepository _productRepository;
    private readonly IQuantityUnitRepository _quantityUnitRepository;

    public RecipeRepository(ILogger<RecipeRepository> logger, IDataBase dataBase,
        IProductRepository productRepository, IQuantityUnitRepository quantityUnitRepository)
    {
        _logger = logger;
        _dataBase = dataBase;
        _productRepository = productRepository;
        _quantityUnitRepository = quantityUnitRepository;
    }

    public Task<List<Recipe>> GetAllRecipesAsync()
    {
        _logger.LogDebug("Getting all recipes");
        var recipeDbos = _dataBase.GetAllRecipes();
        var recipes = recipeDbos.Select(DboToRecipe).ToList();
        return Task.FromResult(recipes);
    }

    public async Task<Recipe?> GetRecipeByIdAsync(EntityId recipeId)
    {
        _logger.LogDebug("Getting recipe by id {RecipeId}", recipeId);
        var recipes = await GetAllRecipesAsync();
        return recipes.FirstOrDefault(r => r.Id == recipeId);
    }

    public async Task<Recipe?> GetRecipeByNameAsync(string recipeName)
    {
        _logger.LogDebug("Getting recipe by name {RecipeName}", recipeName);
        var recipes = await GetAllRecipesAsync();
        return recipes.FirstOrDefault(r => r.Title == recipeName);
    }

    private async Task AddMissingQuantitiesAsync(Recipe recipe)
    {
        var updatedProducts = new Dictionary<EntityId, Product>();

        foreach (var ingredient in recipe.Ingredients)
        {
            var product = await _productRepository.GetProductByIdAsync(ingredient.Product.Id);
            var quantityUnit = ingredient.Quantity.Unit;

            if (!product!.IsAvailableQuantityUnit(quantityUnit))
            {
                product.AddQuantityUnit(quantityUnit);
                updatedProducts[product.Id] = product;
            }
        }

        await _productRepository.AddProductsAsync(updatedProducts.Values.ToList());
    }

    public Task AddRecipesAsync(IEnumerable<Recipe> recipes)
    {
        foreach (var recipe in recipes)
        {
            _logger.LogDebug("Adding recipe {@Recipe}", recipe);
            var recipeDbo = RecipeToDbo(recipe);
            _dataBase.InsertRecipe(recipeDbo);
        }

        return Task.CompletedTask;
    }

    public Task RemoveRecipesByIdAsync(IEnumerable<EntityId> recipeIds)
    {
        throw new NotImplementedException();
    }

    private RecipeDbo RecipeToDbo(Recipe recipe)
    {
        return new RecipeDbo(recipe.Id.ToString(), recipe.Title, recipe.Servings, recipe.CookDuration,
            recipe.Description, recipe.ImageUrl?.ToString(),
            recipe.Ingredients.Select(IngredientToDbo).ToArray(),
            CookingStepsToDbo(recipe.CookingSteps));
    }

    private IngredientDbo IngredientToDbo(Ingredient ingredient)
    {
        return new IngredientDbo(ingredient.Product.Id.ToString(), QuantityToDbo(ingredient.Quantity));
    }

    private QuantityDbo QuantityToDbo(Quantity quantity)
    {
        return new QuantityDbo(quantity.Value, _quantityUnitRepository.GetUnitId(quantity.Unit));
    }

    private static CookingTechniqueDbo CookingStepsToDbo(IEnumerable<CookingStep> cookingSteps)
    {
        return new CookingTechniqueDbo(cookingSteps.Select(s => s.Description).ToArray());
    }

    private Recipe DboToRecipe(RecipeDbo recipeDbo)
    {
        var ingredients = recipeDbo.IngredientDbos
            .Select(DboToIngredient)
            .Where(ing => ing is not null)
            .Cast<Ingredient>()
            .ToList();

        var ingredientGroup = new IngredientGroup(ingredients);
        var cookingTechnique =
            new CookingTechnic(recipeDbo.CookingTechniqueDbo.Steps.Select(step => new CookingStep(step)));

        return new Recipe(new EntityId(recipeDbo.Id), recipeDbo.Title, recipeDbo.Description,
            recipeDbo.Servings, recipeDbo.CookDuration,
            ingredientGroup, cookingTechnique)
        {
            ImageUrl = recipeDbo.ImageUrl is null ? null : new Uri(recipeDbo.ImageUrl)
        };
    }

    private Ingredient? DboToIngredient(IngredientDbo ingredientDbo)
    {
        var product = _productRepository.GetProductByIdAsync(new EntityId(ingredientDbo.ProductId)).Result;
        if (product is null)
        {
            _logger.LogError("Product with id {ProductId} not found", ingredientDbo.ProductId);
            return null;
        }

        var quantity = DboToQuantity(ingredientDbo.QuantityDbo);
        return quantity is null ? null : new Ingredient(product, quantity);
    }

    private Quantity? DboToQuantity(QuantityDbo quantityDbo)
    {
        var unit = _quantityUnitRepository.GetUnitById(quantityDbo.UnitId);
        if (unit is null)
        {
            _logger.LogError("Unit with id {UnitId} not found", quantityDbo.UnitId);
            return null;
        }

        return new Quantity(quantityDbo.Value, unit);
    }
}