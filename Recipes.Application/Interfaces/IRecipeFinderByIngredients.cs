using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Application.Interfaces;

public interface IRecipeFinderByAvailableIngredients
{
    IEnumerable<Recipe> FindRecipesByAvailableIngredients(IEnumerable<Recipe> recipes, IngredientGroup ingredients);
}

