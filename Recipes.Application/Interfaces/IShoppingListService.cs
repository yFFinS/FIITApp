using Recipes.Domain.Entities.ShoppingListAggregate;
using Recipes.Domain.IngredientsAggregate;

namespace Recipes.Application.Interfaces;

public interface IShoppingListService
{
    IngredientGroup GetMissingIngredients(ShoppingList shoppingList);
}