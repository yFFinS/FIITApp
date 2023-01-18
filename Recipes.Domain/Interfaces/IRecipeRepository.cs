using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Interfaces;

public interface IRecipeRepository
{
    List<Recipe> GetAllRecipes(bool onlyGlobal = false);
    Recipe? GetRecipeById(EntityId recipeId);
    Recipe? GetRecipeByName(string recipeName);
    List<Recipe> GetRecipesBySubstring(string substring);
    void AddRecipes(IEnumerable<Recipe> recipes, bool useUserDatabase = false);
    void RemoveRecipesById(IEnumerable<EntityId> recipeIds);
}