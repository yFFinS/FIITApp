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
        var ingredientsDictionary = ingredients.ToDictionary(ingredient => ingredient.ProductId);
        return recipes
            .Where(recipe =>
                !recipe.Ingredients.Any(ingredient =>
                    !ingredientsDictionary.ContainsKey(ingredient.ProductId)));
    }

    public IEnumerable<Recipe> PickRecipesByAvailableIngredients(
        IEnumerable<Recipe> recipes,
        IngredientGroup ingredients)
    {
        var ingredientsDictionary = ingredients.ToDictionary(ingredient => ingredient.ProductId);
        return recipes
            .Where(recipe =>
                !recipe.Ingredients.Any(ingredient =>
                    !ingredientsDictionary.ContainsKey(ingredient.ProductId) ||
                    ingredientsDictionary[ingredient.ProductId].Quantity < ingredient.Quantity));
    }

    public IEnumerable<Recipe> PickRecipesByAvailableIngredientsWithMarginOfError(
        IEnumerable<Recipe> recipes,
        IngredientGroup ingredients,
        double margin)
    {
        margin = Guard.Against.NegativeOrZero(margin);
        var ingredientsDictionary = ingredients.ToDictionary(ingredient => ingredient.ProductId);
        return recipes
            .Where(recipe =>
                !recipe.Ingredients.Any(ingredient =>
                    !ingredientsDictionary.ContainsKey(ingredient.ProductId) ||
                    ingredientsDictionary[ingredient.ProductId].Quantity
                        .LessThanWithMargin(ingredient.Quantity, margin)));
    }
}
