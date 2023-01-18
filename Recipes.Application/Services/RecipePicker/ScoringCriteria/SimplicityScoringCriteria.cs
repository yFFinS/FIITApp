using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Interfaces;

namespace Recipes.Application.Services.RecipePicker.ScoringCriteria;

public record SimplicityCriteriaScores(int MaxIngredientPenaltyCount, double IngredientPenalty,
    int MaxTimePenaltyCount, double CookingTimeSecondPenalty) : IOptions;

public class SimplicityScoringCriteria : IScoringCriteria
{
    private readonly SimplicityCriteriaScores _scores;

    public SimplicityScoringCriteria(SimplicityCriteriaScores scores)
    {
        _scores = scores;
    }

    public double ScoreRecipe(Recipe recipe, RecipeFilter recipeFilter)
    {
        var score = 0.0;

        var remainingIngredients = Math.Max(0, _scores.MaxIngredientPenaltyCount - recipe.Ingredients.Count);
        var remainingTime = Math.Max(0, _scores.MaxTimePenaltyCount - (int)recipe.CookDuration.TotalSeconds);

        score += remainingIngredients * _scores.IngredientPenalty;
        score += remainingTime * _scores.CookingTimeSecondPenalty;

        return score;
    }
}