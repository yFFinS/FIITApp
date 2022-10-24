using Recipes.Application.Services.RecipePicker;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;

namespace Recipes.Application.Interfaces;

public interface IRecipePicker
{
    Task<List<Recipe>> PickRecipes(RecipeFilter filter);
}