using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;

namespace Recipes.Application.Interfaces;

public interface IRecipesFinder
{
    Task<List<Recipe>> FindRecipeAsync(IngredientGroup ingredientGroup);
    Task<List<Recipe>> FindRecipeAsync(IngredientGroup ingredientGroup, int maxResults);
}