using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Application.Services.RecipePicker.ScoringCritirias;

public interface IScoringCriteria
{
    double ScoreRecipe(Recipe recipe, RecipeFilter recipeFilter);
}