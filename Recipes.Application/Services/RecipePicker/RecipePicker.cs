using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Interfaces;

namespace Recipes.Application.Services.RecipePicker;

public class RecipePicker : IRecipePicker
{
    private readonly ILogger<RecipePicker> _logger;
    private readonly IRecipeRepository _recipeRepository;

    public RecipePicker(ILogger<RecipePicker> logger, IRecipeRepository recipeRepository)
    {
        _logger = logger;
        _recipeRepository = recipeRepository;
    }

    public async Task<List<Recipe>> PickRecipes(RecipeFilter filter)
    {
        _logger.LogInformation("Picking recipes for filter {@Filter}", filter);

        var recipes = await _recipeRepository.GetRecipesAsync();
        var allowedRecipes = recipes
            .Where(rec => filter.MaxCookDuration is null || rec.CookDuration <= filter.MaxCookDuration)
            .Where(rec => IsAllProductsAllowed(rec, filter));

        var scoredRecipes = allowedRecipes
            .Select(rec => new { Recipe = rec, Score = ScoreRecipe(rec, filter) })
            .OrderByDescending(rec => rec.Score);

        return scoredRecipes.Select(rec => rec.Recipe)
            .Take(filter.MaxRecipes)
            .ToList();
    }

    private bool IsAllProductsAllowed(Recipe recipe, RecipeFilter filter)
    {
        return recipe.Ingredients.Select(ing => ing.Product).All(filter.IsAllowed);
    }

    private double ScoreRecipe(Recipe recipe, RecipeFilter filter)
    {
        const double notEnoughProductsPenalty = -25.0;
        const double hasProductScore = 10.0;
        const double allOptionsSatisfiedScore = 100.0;

        var satisfiedOptionsCount = 0;

        var score = 0.0;
        foreach (var ingredient in recipe.Ingredients)
        {
            var optionForProduct = filter.GetOption(ingredient.Product);
            if (optionForProduct is null)
            {
                continue;
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