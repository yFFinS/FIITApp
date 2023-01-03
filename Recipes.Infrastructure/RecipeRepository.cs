using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;

namespace Recipes.Infrastructure;

public class RecipeRepository : IRecipeRepository
{
    private readonly ILogger<RecipeRepository> _logger;
    private readonly IDataBase _dataBase;
    private readonly IProductRepository _productRepository;

    public RecipeRepository(ILogger<RecipeRepository> logger, IDataBase dataBase, IProductRepository productRepository)
    {
        _logger = logger;
        _dataBase = dataBase;
        _productRepository = productRepository;
    }

    public Task<List<Recipe>> GetAllRecipesAsync()
    {
        _logger.LogDebug("Getting all recipes");
        var recipes = _dataBase.GetAllRecipes();
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
            var product = await _productRepository.GetProductByIdAsync(ingredient.ProductId);
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
            _dataBase.InsertRecipe(recipe);
        }

        return Task.CompletedTask;
    }

    public Task RemoveRecipesByIdAsync(IEnumerable<EntityId> recipeIds)
    {
        throw new NotImplementedException();
    }
}