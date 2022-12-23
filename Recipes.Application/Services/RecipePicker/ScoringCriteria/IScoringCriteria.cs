using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Application.Services.RecipePicker.ScoringCriteria;

public interface IScoringCriteria
{
    double ScoreRecipe(Recipe recipe, RecipeFilter recipeFilter);
}