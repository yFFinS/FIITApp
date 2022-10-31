using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;

namespace Recipes.Application.Interfaces;

public interface IRecipeIngredientsMerger
{
    IngredientGroup MergeRequiredIngredients(IEnumerable<Recipe> recipes);
}