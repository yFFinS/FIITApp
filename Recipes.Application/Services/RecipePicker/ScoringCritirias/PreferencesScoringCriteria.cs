using Recipes.Application.Services.Preferences;
using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Application.Services.RecipePicker.ScoringCritirias;

// const double notEnoughProductsPenalty = -25.0;
// const double hasProductScore = 10.0;
// const double allOptionsSatisfiedScore = 100.0;
// const double dislikedProductPenalty = -50.0;
// const double likedProductScore = 50.0;
// const double dislikedRecipePenalty = -200.0;
// const double likedRecipeScore = 100.0;

public record PreferencesScores(double LikedProductScore, double DislikedProductPenalty,
    double LikedRecipeScore, double DislikedRecipePenalty);

public class PreferencesScoringCriteria : IScoringCriteria
{
    private readonly IPreferenceService _preferenceService;
    private readonly PreferencesScores _preferencesScores;

    public PreferencesScoringCriteria(IPreferenceService preferenceService, PreferencesScores preferencesScores)
    {
        _preferenceService = preferenceService;
        _preferencesScores = preferencesScores;
    }

    public double ScoreRecipe(Recipe recipe, RecipeFilter recipeFilter)
    {
        var score = 0.0;
        var preferences = _preferenceService.GetPreferences();

        if (preferences.IsLikedRecipe(recipe.Id))
        {
            score += _preferencesScores.LikedRecipeScore;
        }
        else if (preferences.IsDislikedRecipe(recipe.Id))
        {
            score += _preferencesScores.DislikedRecipePenalty;
        }

        foreach (var ingredient in recipe.Ingredients)
        {
            var productId = ingredient.ProductId;
            if (preferences.IsLikedProduct(productId))
            {
                score += _preferencesScores.LikedProductScore;
            }
            else if (preferences.IsDislikedProduct(productId))
            {
                score += _preferencesScores.DislikedProductPenalty;
            }
        }

        return score;
    }
}