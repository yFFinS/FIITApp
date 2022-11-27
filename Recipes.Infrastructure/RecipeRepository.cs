using Microsoft.Extensions.Logging;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;

namespace Recipes.Infrastructure;

public class RecipeRepository : IRecipeRepository
{
    private readonly ILogger<RecipeRepository> _logger;

    public RecipeRepository(ILogger<RecipeRepository> logger)
    {
        _logger = logger;
    }

    public Task<List<Recipe>> GetAllRecipesAsync()
    {
        _logger.LogDebug("Getting all recipes");
        var recipes = DataBase.GetAllRecipes();
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

    public Task AddRecipesAsync(IEnumerable<Recipe> recipes)
    {
        foreach (var recipe in recipes)
        {
            _logger.LogDebug("Adding recipe {@Recipe}", recipe);
            DataBase.InsertRecipe(recipe);
        }
        
        return Task.CompletedTask;
    }

    public Task RemoveRecipesByIdAsync(IEnumerable<EntityId> recipeIds)
    {
        throw new NotImplementedException();
    }
}