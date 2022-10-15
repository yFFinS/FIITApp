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

    //public IEnumerable<Recipe> PickRecipesByAvailableIngredients(
    //    IEnumerable<Recipe> recipes,
    //    IngredientGroup ingredients,
    //    double margin = 0)
    //{
    //    margin = Guard.Against.Negative(margin);
    //    return recipes
    //        .Where(recipe =>
    //            !recipe.Ingredients.Any(recipeIngredient =>
    //                margin == 0 ? 
    //                    ByIngredient(ingredients, recipeIngredient) :
    //                    ByIngredientWithMargin(ingredients, recipeIngredient, margin)));
    //}

    public IEnumerable<Recipe> PickRecipesByAvailableIngredients(
        IEnumerable<Recipe> recipes,
        IngredientGroup ingredients)
    {
        return recipes
            .Where(recipe =>
                !recipe.Ingredients.Any(recipeIngredient =>
                        ByIngredient(ingredients, recipeIngredient)));
    }

    public IEnumerable<Recipe> PickRecipesByAvailableIngredientsWithMargin(
        IEnumerable<Recipe> recipes,
        IngredientGroup ingredients,
        double margin)
    {
        margin = Guard.Against.Negative(margin);
        return recipes
            .Where(recipe =>
                !recipe.Ingredients.Any(recipeIngredient =>
                        ByIngredientWithMargin(ingredients, recipeIngredient, margin)));
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

    private static bool ByIngredientWithMargin(IngredientGroup ingredients, Ingredient recipeIngredient, double margin)
    {
        var ingredient = ingredients.TryGetByProductId(recipeIngredient.ProductId);
        if (ingredient is not null)
        {
            return ingredient.Quantity.LessThanWithMargin(
                recipeIngredient.Quantity,
                margin);
        }
        return false;
    }
}
