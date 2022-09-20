using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;

namespace Recipes.Application.Services;

public class RecipeIngredientsMerger : IRecipeIngredientsMerger
{
    private readonly ILogger<RecipeIngredientsMerger> _logger;
    private readonly IIngredientGroupEditService _ingredientGroupEditService;

    public RecipeIngredientsMerger(ILogger<RecipeIngredientsMerger> logger,
        IIngredientGroupEditService ingredientGroupEditService)
    {
        _logger = logger;
        _ingredientGroupEditService = ingredientGroupEditService;
    }

    public IngredientGroup MergeRequiredIngredients(IEnumerable<Recipe> recipes)
    {
        var ingredients = new IngredientGroup();

        foreach (var recipe in recipes)
        {
            var recipeIngredients = recipe.Ingredients;
            InsertIngredients(recipeIngredients, ingredients);
            _logger.LogInformation("Merged recipe {RecipeId} into ingredient group", recipe.Id);
        }

        return ingredients;
    }

    private void InsertIngredients(IEnumerable<Ingredient> source, IngredientGroup destination)
    {
        foreach (var ingredient in source)
        {
            _ingredientGroupEditService.AddByQuantity(destination, ingredient);
        }
    }
}