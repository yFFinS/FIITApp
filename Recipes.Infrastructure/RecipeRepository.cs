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

    private readonly Dictionary<QuantityUnit, HashSet<QuantityUnit>> _convertibleTo = new();

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

    public async Task<List<Recipe>> GetRecipesByPrefixAsync(string prefix)
    {
        prefix = prefix.ToLower();
        _logger.LogDebug("Getting recipe by prefix {Prefix}", prefix);
        var recipes = await GetAllRecipesAsync();
        return recipes.Where(r => r.Title.Split(' ').Any(s => s.StartsWith(prefix))).ToList();
    }

    private async Task AddMissingQuantitiesAsync(Recipe recipe)
    {
        InitConversionTable();

        var updatedProducts = new Dictionary<EntityId, Product>();

        foreach (var ingredient in recipe.Ingredients)
        {
            var product = await _productRepository.GetProductByIdAsync(ingredient.Product.Id);
            var quantityUnit = ingredient.Quantity.Unit;

            var convertibleTo = _convertibleTo.TryGetValue(quantityUnit, out var convertibleToSet)
                ? convertibleToSet
                : new HashSet<QuantityUnit>();

            foreach (var unit in convertibleTo.Append(quantityUnit)
                         .Where(unit => !product!.IsAvailableQuantityUnit(unit)))
            {
                product!.AddQuantityUnit(unit);
                updatedProducts[product.Id] = product;
            }
        }

        await _productRepository.AddProductsAsync(updatedProducts.Values.ToList());
    }

    private void InitConversionTable()
    {
        if (_convertibleTo.Count > 0)
        {
            return;
        }

        var quantityUnits = _quantityUnitRepository.GetAllUnits();
        foreach (var from in quantityUnits)
        {
            foreach (var to in quantityUnits)
            {
                if (from == to)
                {
                    continue;
                }

                if (!from.CanConvertTo(to))
                {
                    continue;
                }

                if (!_convertibleTo.ContainsKey(from))
                {
                    _convertibleTo[from] = new HashSet<QuantityUnit>();
                }

                _convertibleTo[from].Add(to);
            }
        }
    }

    public async Task AddRecipesAsync(IEnumerable<Recipe> recipes)
    {
        foreach (var recipe in recipes)
        {
            _logger.LogDebug("Adding recipe {@Recipe}", recipe);

            await AddMissingQuantitiesAsync(recipe);

            var recipeDbo = RecipeToDbo(recipe);
            _dataBase.InsertRecipe(recipeDbo);
        }
    }

    public Task RemoveRecipesByIdAsync(IEnumerable<EntityId> recipeIds)
    {
        throw new NotImplementedException();
    }

    private RecipeDbo RecipeToDbo(Recipe recipe)
    {
        return new RecipeDbo
        {
            Id = recipe.Id.ToString(),
            Title = recipe.Title,
            Servings = recipe.Servings,
            CookDuration = recipe.CookDuration,
            Description = recipe.Description,
            ImageUrl = recipe.ImageUrl?.ToString(),
            IngredientDbos = recipe.Ingredients.Select(IngredientToDbo).ToArray(),
            CookingTechniqueDbo = CookingStepsToDbo(recipe.CookingSteps)
        };
    }

    private IngredientDbo IngredientToDbo(Ingredient ingredient)
    {
        return new IngredientDbo
        {
            ProductId = ingredient.Product.Id.ToString(),
            QuantityDbo = QuantityToDbo(ingredient.Quantity)
        };
    }

    private QuantityDbo QuantityToDbo(Quantity quantity)
    {
        return new QuantityDbo
        {
            Value = quantity.Value,
            UnitId = _quantityUnitRepository.GetUnitId(quantity.Unit)
        };
    }

    private static CookingTechniqueDbo CookingStepsToDbo(IEnumerable<CookingStep> cookingSteps)
    {
        return new CookingTechniqueDbo
        {
            Steps = cookingSteps.Select(s => s.Description).ToArray()
        };
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