using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.ShoppingListAggregate;
using Recipes.Domain.IngredientsAggregate;

namespace Recipes.Application.Services;

public class ShoppingListService : IShoppingListService
{
    private readonly ILogger<ShoppingListService> _logger;
    private readonly IIngredientGroupEditService _ingredientGroupEdit;
    private readonly IRecipeIngredientsMerger _recipeIngredientsMerger;

    public ShoppingListService(ILogger<ShoppingListService> logger, IIngredientGroupEditService ingredientGroupEdit,
        IRecipeIngredientsMerger recipeIngredientsMerger)
    {
        _logger = logger;
        _ingredientGroupEdit = ingredientGroupEdit;
        _recipeIngredientsMerger = recipeIngredientsMerger;
    }


    public IngredientGroup GetMissingIngredients(ShoppingList shoppingList)
    {
        var recipes = shoppingList.Recipes;

        _logger.LogInformation("Merging ingredients from {RecipeCount} recipes", recipes.Count);
        var requiredIngredients = _recipeIngredientsMerger.MergeRequiredIngredients(recipes);
        var availableIngredients = shoppingList.Ingredients;

        _logger.LogInformation("Calculating missing ingredients");
        foreach (var availableIngredient in availableIngredients)
        {
            _ = _ingredientGroupEdit.RemoveByQuantity(requiredIngredients, availableIngredient);
        }

        _logger.LogInformation("Missing ingredients calculated. {MissingIngredientCount} missing ingredients found",
            requiredIngredients.Count);
        return requiredIngredients;
    }
}