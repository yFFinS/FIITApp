using Recipes.Domain.Entities.IngredientAggregate;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Interfaces;

namespace Recipes.Application.Services;

public class RecipesFinder
{
    private readonly IRecipeRepository _recipeRepository;

    public RecipesFinder(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<List<Recipe>> FindRecipeAsync(Ingredients ingredients)
    {
        return await _recipeRepository.GetRecipesAsync(ingredients);
    }

    public async Task<List<Recipe>> FindRecipeAsync(Ingredients ingredients, int maxResults)
    {
        return await _recipeRepository.GetRecipesAsync(ingredients, maxResults);
    }
}