using Recipes.Application.Services.Preferences;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Interfaces;

namespace Recipes.Application.Services.RecipePicker.ScoringCriteria;

public record PreferencesCriteriaScores(double LikedProductScore, double DislikedProductPenalty,
    double LikedRecipeScore, double DislikedRecipePenalty) : IOptions;

public class PreferencesScoringCriteria : IScoringCriteria
{
    private readonly IPreferenceService _preferenceService;
    private readonly PreferencesCriteriaScores _preferencesCriteriaScores;

    public PreferencesScoringCriteria(IPreferenceService preferenceService,
        PreferencesCriteriaScores preferencesCriteriaScores)
    {
        _preferenceService = preferenceService;
        _preferencesCriteriaScores = preferencesCriteriaScores;
    }

    public double ScoreRecipe(Recipe recipe, RecipeFilter recipeFilter)
    {
        var score = 0.0;
        var preferences = _preferenceService.GetPreferences();

        if (preferences.IsLikedRecipe(recipe.Id))
        {
            score += _preferencesCriteriaScores.LikedRecipeScore;
        }
        else if (preferences.IsDislikedRecipe(recipe.Id))
        {
            score += _preferencesCriteriaScores.DislikedRecipePenalty;
        }

        foreach (var ingredient in recipe.Ingredients)
        {
            var productId = ingredient.Product.Id;
            if (preferences.IsLikedProduct(productId))
            {
                score += _preferencesCriteriaScores.LikedProductScore;
            }
            else if (preferences.IsDislikedProduct(productId))
            {
                score += _preferencesCriteriaScores.DislikedProductPenalty;
            }
        }

        return score;
    }
}