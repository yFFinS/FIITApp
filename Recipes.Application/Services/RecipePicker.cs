using Ardalis.GuardClauses;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;

namespace Recipes.Application.Services;

public class RecipePicker : IRecipePicker
{
    public IEnumerable<Recipe> PickRecipesByIngredients(
        IEnumerable<Recipe> recipes,
        IngredientGroup ingredients)
    {
        return recipes
            .Where(recipe =>
                !recipe.Ingredients.Any(ingredient =>
                    ingredients.TryGetByProductId(ingredient.ProductId) is not null));
    }

    public IEnumerable<Recipe> PickRecipesByAvailableIngredients(
        IEnumerable<Recipe> recipes,
        IngredientGroup ingredients)
    {
        return recipes
            .Where(recipe =>
                !recipe.Ingredients.Any(recipeIngredient =>
                        ByIngredient(ingredients, recipeIngredient)));
    }

    public IEnumerable<Recipe> PickRecipesByAvailableIngredientsWithRatio(
        IEnumerable<Recipe> recipes,
        IngredientGroup ingredients,
        double ratio)
    {
        ratio = Guard.Against.Negative(ratio);
        return recipes
            .Where(recipe =>
                !recipe.Ingredients.Any(recipeIngredient =>
                        ByIngredientWithRatio(ingredients, recipeIngredient, ratio)));
    }

    private static bool ByIngredient(IngredientGroup ingredients, Ingredient recipeIngredient)
    {
        var ingredient = ingredients.TryGetByProductId(recipeIngredient.ProductId);
        if (ingredient is not null)
        {
            return ingredient.Quantity < recipeIngredient.Quantity;
        }
        return false;
    }

    private static bool ByIngredientWithRatio(IngredientGroup ingredients, Ingredient recipeIngredient, double ratio)
    {
        var ingredient = ingredients.TryGetByProductId(recipeIngredient.ProductId);
        if (ingredient is not null)
        {
            return ingredient.Quantity.LessThanWithRatio(
                recipeIngredient.Quantity,
                ratio);
        }
        return false;
    }
}
