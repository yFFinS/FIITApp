using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.ShoppingListAggregate;

public class RecipeNonexistentException : DomainException
{
    public RecipeNonexistentException(EntityId recipeId) : base($"Recipe with id {recipeId} does not exist.")
    {
    }
}