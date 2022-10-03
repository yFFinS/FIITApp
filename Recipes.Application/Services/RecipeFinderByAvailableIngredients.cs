using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Application.Services;

public class RecipeFinderByAvailableIngredients : IRecipeFinderByAvailableIngredients
{
    public IEnumerable<Recipe> FindRecipesByAvailableIngredients(IEnumerable<Recipe> recipes, IngredientGroup ingredients)
    {
        var resultRecipes = new List<Recipe>();
        var ingredientsCollection = ingredients.AsReadOnlyCollection();
        foreach (var recipe in recipes)
        {
            var recipeContainsAll = true;
            foreach (var ingredient in ingredientsCollection)
            {
                var product = ingredient.Product.Id;
                var productQuantity = ingredient.Quantity;
                var contains = false;
                foreach (var recipeIngredient in recipe.Ingredients)
                {
                    var recipeProduct = recipeIngredient.Product.Id;
                    var recipeProductQuantity = recipeIngredient.Quantity;
                    if (recipeProduct == product && productQuantity >= recipeProductQuantity)
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
