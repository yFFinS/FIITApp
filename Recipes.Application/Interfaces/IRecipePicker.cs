using Recipes.Application.Services.RecipePicker;
using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Application.Interfaces;

public interface IRecipePicker
{
    Task<List<Recipe>> PickRecipes(RecipeFilter filter);
}