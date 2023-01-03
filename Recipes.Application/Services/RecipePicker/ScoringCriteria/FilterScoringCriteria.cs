using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Interfaces;

namespace Recipes.Application.Services.RecipePicker.ScoringCriteria;

public record FilterCriteriaScores(double NotEnoughProductPenalty,
    double HasEnoughProductScore, double AllOptionsSatisfiedScore) : IOptions;

public class FilterScoringCriteria : IScoringCriteria
{
    private readonly FilterCriteriaScores _scores;

    public FilterScoringCriteria(FilterCriteriaScores scores)
    {
        _scores = scores;
    }

    public double ScoreRecipe(Recipe recipe, RecipeFilter recipeFilter)
    {
        var score = 0.0;
        var satisfiedOptions = 0;

        foreach (var ingredient in recipe.Ingredients)
        {
            var optionForProduct = recipeFilter.GetOption(ingredient.ProductId);
            if (optionForProduct is null)
            {
                continue;
            }

            if (optionForProduct.Quantity is not null && optionForProduct.Quantity < ingredient.Quantity)
            {
                score += _scores.NotEnoughProductPenalty;
            }
            else
            {
                score += _scores.HasEnoughProductScore;
            }
        }

        if (satisfiedOptions == recipeFilter.OptionCount)
        {
            score += _scores.AllOptionsSatisfiedScore;
        }

        return score;
    }
}