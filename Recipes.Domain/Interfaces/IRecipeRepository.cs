using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;

namespace Recipes.Domain.Interfaces;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetRecipesAsync(IngredientGroup availableIngredientGroup);

    Task<List<Recipe>> GetRecipesAsync(IngredientGroup availableIngredientGroup, int maxResults);
}