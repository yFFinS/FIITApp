using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.Interfaces;

namespace Recipes.Application.Services;

public class RecipesFinder
{
    private readonly IRecipeRepository _recipeRepository;

    public RecipesFinder(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<List<Recipe>> FindRecipeAsync(IngredientGroup ingredientGroup)
    {
        return await _recipeRepository.GetRecipesAsync(ingredientGroup);
    }

    public async Task<List<Recipe>> FindRecipeAsync(IngredientGroup ingredientGroup, int maxResults)
    {
        return await _recipeRepository.GetRecipesAsync(ingredientGroup, maxResults);
    }
}