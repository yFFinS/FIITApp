using Recipes.Domain.Entities.IngredientAggregate;
using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Domain.Interfaces;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetRecipesAsync(Ingredients availableIngredients);

    Task<List<Recipe>> GetRecipesAsync(Ingredients availableIngredients, int maxResults);
}