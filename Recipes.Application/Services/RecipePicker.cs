using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Application.Services;

public class RecipePicker : IRecipePicker
{
    public IEnumerable<Recipe> PickRecipesByAvailableIngredients(IEnumerable<Recipe> recipes, IngredientGroup ingredients)
    {
        return PickRecipesByParameters(recipes, ingredients, ByAvailableIngredients);
    }

    public IEnumerable<Recipe> PickRecipesByIngredients(IEnumerable<Recipe> recipes, IngredientGroup ingredients)
    {
        return PickRecipesByParameters(recipes, ingredients, ByIngredients);
    }

    private bool ByAvailableIngredients(Ingredient ingredient, Ingredient recipeIngredient)
    {
        var product = ingredient.Product.Id;
        var productQuantity = ingredient.Quantity;
        var recipeProduct = recipeIngredient.Product.Id;
        var recipeProductQuantity = recipeIngredient.Quantity;
        return recipeProduct == product && productQuantity >= recipeProductQuantity;
    }

    private bool ByIngredients(Ingredient ingredient, Ingredient recipeIngredient)
    {
        var product = ingredient.Product.Id;
        var recipeProduct = recipeIngredient.Product.Id;
        return recipeProduct == product;
    }

    private IEnumerable<Recipe> PickRecipesByParameters(IEnumerable<Recipe> recipes, IngredientGroup ingredients,
        Func<Ingredient, Ingredient, bool> comparator)
    {
        var resultRecipes = new List<Recipe>();
        var ingredientsCollection = ingredients.AsReadOnlyCollection();
        foreach (var recipe in recipes)
        {
            var recipeContainsAll = true;
            foreach (var ingredient in ingredientsCollection)
            {
                var contains = false;
                foreach (var recipeIngredient in recipe.Ingredients)
                {
                    if (comparator(ingredient, recipeIngredient))
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                {
                    recipeContainsAll = false;
                    break;
                }
            }
            if (recipeContainsAll)
            {
                resultRecipes.Add(recipe);
            }
        }
        return resultRecipes;
    }
}
