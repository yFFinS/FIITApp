using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;

namespace Recipes.Application.Interfaces;

public interface IRecipePicker
{
    IEnumerable<Recipe> PickRecipesByIngredients(
        IEnumerable<Recipe> recipes,
        IngredientGroup ingredients);

    IEnumerable<Recipe> PickRecipesByAvailableIngredients(
        IEnumerable<Recipe> recipes,
        IngredientGroup ingredients);

    IEnumerable<Recipe> PickRecipesByAvailableIngredientsWithMarginOfError(
        IEnumerable<Recipe> recipes,
        IngredientGroup ingredients,
        double margin);
}

