using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Application.Services.RecipePicker.ScoringCriteria;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Interfaces;

namespace Recipes.Application.Services.RecipePicker;

public class RecipePicker : IRecipePicker
{
    private const double MinScore = -500;
    
    private readonly ILogger<RecipePicker> _logger;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IReadOnlyList<IScoringCriteria> _scoringCriteria;

    public RecipePicker(ILogger<RecipePicker> logger, IRecipeRepository recipeRepository,
        IReadOnlyList<IScoringCriteria> scoringCriteria)
    {
        _logger = logger;
        _recipeRepository = recipeRepository;
        _scoringCriteria = scoringCriteria;
    }

    public List<Recipe> PickRecipes(RecipeFilter filter)
    {
        _logger.LogInformation("Picking recipes for filter {@Filter}", filter);

        var recipes = _recipeRepository.GetAllRecipes();
        var allowedRecipes = GetAllowedRecipes(filter, recipes);
        var scoredRecipes = ScoreRecipes(filter, allowedRecipes);

        return scoredRecipes.Where(rec => rec.Score >= MinScore)
            .Select(rec => rec.Recipe)
            .Take(filter.MaxRecipes)
            .ToList();
    }

    private IOrderedEnumerable<(Recipe Recipe, double Score)> ScoreRecipes(RecipeFilter filter,
        IEnumerable<Recipe> allowedRecipes)
    {
        return allowedRecipes
            .Select(rec => (Recipe: rec, Score: ScoreRecipe(rec, filter)))
            .OrderByDescending(rec => rec.Score);
    }

    private static IEnumerable<Recipe> GetAllowedRecipes(RecipeFilter filter, List<Recipe> recipes)
    {
        return recipes
            .Where(rec => filter.MaxCookDuration is null || rec.CookDuration <= filter.MaxCookDuration)
            .Where(rec => IsAllProductsAllowed(rec, filter));
    }

    private static bool IsAllProductsAllowed(Recipe recipe, RecipeFilter filter)
    {
        return recipe.Ingredients.Select(ing => ing.Product.Id).All(filter.IsAllowed);
    }

    private double ScoreRecipe(Recipe recipe, RecipeFilter filter)
    {
        return _scoringCriteria.Sum(c => c.ScoreRecipe(recipe, filter));
    }
}