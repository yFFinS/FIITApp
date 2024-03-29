using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Interfaces;

namespace Recipes.Application.Services.RecipePicker.ScoringCriteria;

public record FilterCriteriaScores(double NotEnoughProductPenalty,
    double HasEnoughProductScore, double AllOptionsSatisfiedScore,
    double ProductNotUsedPenalty, double NoOptionsSatisfiedPenalty) : IOptions;

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

        var optionsCount = 0;

        foreach (var ingredient in recipe.Ingredients)
        {
            var optionForProduct = recipeFilter.GetOption(ingredient.Product.Id);
            if (optionForProduct is null)
            {
                continue;
            }

            optionsCount++;

            if (optionForProduct.Quantity is not null && optionForProduct.Quantity < ingredient.Quantity)
            {
                score += _scores.NotEnoughProductPenalty;
            }
            else
            {
                score += _scores.HasEnoughProductScore;
                satisfiedOptions++;
            }
        }

        var satisfiedOptionsRatio = (double)satisfiedOptions / optionsCount;
        score += _scores.AllOptionsSatisfiedScore * satisfiedOptionsRatio;

        var notUsedOptionsCount = recipeFilter.OptionCount - optionsCount;
        score += _scores.ProductNotUsedPenalty * notUsedOptionsCount;

        return score;
    }
}