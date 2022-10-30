using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Application.Services.Preferences;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Interfaces;

namespace Recipes.Application.Services.RecipePicker;

public class RecipePicker : IRecipePicker
{
    private readonly ILogger<RecipePicker> _logger;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IPreferenceService _preferenceService;

    public RecipePicker(ILogger<RecipePicker> logger, IRecipeRepository recipeRepository,
        IPreferenceService preferenceService)
    {
        _logger = logger;
        _recipeRepository = recipeRepository;
        _preferenceService = preferenceService;
    }

    public async Task<List<Recipe>> PickRecipes(RecipeFilter filter)
    {
        _logger.LogInformation("Picking recipes for filter {@Filter}", filter);

        var recipes = await _recipeRepository.GetRecipesAsync();
        var allowedRecipes = recipes
            .Where(rec => filter.MaxCookDuration is null || rec.CookDuration <= filter.MaxCookDuration)
            .Where(rec => IsAllProductsAllowed(rec, filter));

        var preferences = _preferenceService.GetPreferences();

        var scoredRecipes = allowedRecipes
            .Select(rec => new { Recipe = rec, Score = ScoreRecipe(rec, filter, preferences) })
            .OrderByDescending(rec => rec.Score);

        return scoredRecipes.Select(rec => rec.Recipe)
            .Take(filter.MaxRecipes)
            .ToList();
    }

    private static bool IsAllProductsAllowed(Recipe recipe, RecipeFilter filter)
    {
        return recipe.Ingredients.Select(ing => ing.ProductId).All(filter.IsAllowed);
    }

    private double ScoreRecipe(Recipe recipe, RecipeFilter filter, Preferences.Preferences preferences)
    {
        const double notEnoughProductsPenalty = -25.0;
        const double hasProductScore = 10.0;
        const double allOptionsSatisfiedScore = 100.0;
        const double dislikedProductPenalty = -50.0;
        const double likedProductScore = 50.0;
        const double dislikedRecipePenalty = -200.0;
        const double likedRecipeScore = 100.0;

        var satisfiedOptionsCount = 0;

        var score = 0.0;

        if (preferences.IsLikedRecipe(recipe.Id))
        {
            score += likedRecipeScore;
        }
        else if (preferences.IsDislikedRecipe(recipe.Id))
        {
            score += dislikedRecipePenalty;
        }

        foreach (var ingredient in recipe.Ingredients)
        {
            var optionForProduct = filter.GetOption(ingredient.ProductId);
            if (optionForProduct is null)
            {
                continue;
            }

            if (preferences.IsLikedProduct(ingredient.ProductId))
            {
                score += likedProductScore;
            }
            else if (preferences.IsDislikedProduct(ingredient.ProductId))
            {
                score += dislikedProductPenalty;
            }

            if (optionForProduct.Quantity is not null && optionForProduct.Quantity < ingredient.Quantity)
            {
                score += notEnoughProductsPenalty;
            }
            else
            {
                score += hasProductScore;
                satisfiedOptionsCount++;
            }
        }

        if (satisfiedOptionsCount == filter.OptionCount)
        {
            score += allOptionsSatisfiedScore;
        }

        return score;
    }
}