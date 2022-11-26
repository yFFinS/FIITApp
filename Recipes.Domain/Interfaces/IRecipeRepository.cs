using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Interfaces;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetRecipesAsync();
    Task<Recipe?> GetRecipeByIdAsync(EntityId recipeId);
    Task<Recipe?> GetRecipeByNameAsync(string recipeName);
    Task AddRecipesAsync(IEnumerable<Recipe> recipes);
    Task RemoveRecipesByIdAsync(IEnumerable<EntityId> recipeIds);
}