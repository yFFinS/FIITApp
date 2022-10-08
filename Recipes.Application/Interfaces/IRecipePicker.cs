using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Application.Interfaces;

public interface IRecipePicker
{
    IEnumerable<Recipe> PickRecipesByIngredients(IEnumerable<Recipe> recipes, IngredientGroup ingredients);

    IEnumerable<Recipe> PickRecipesByAvailableIngredients(IEnumerable<Recipe> recipes, IngredientGroup ingredients);
}

