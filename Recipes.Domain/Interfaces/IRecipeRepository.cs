using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Interfaces;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetAllRecipesAsync();
    Task<Recipe?> GetRecipeByIdAsync(EntityId recipeId);
    Task<Recipe?> GetRecipeByNameAsync(string recipeName);
    Task<List<Recipe>> GetRecipesByPrefixAsync(string prefix);
    Task AddRecipesAsync(IEnumerable<Recipe> recipes, bool useUserDatabase = false);
    Task RemoveRecipesByIdAsync(IEnumerable<EntityId> recipeIds);
}