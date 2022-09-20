using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.ShoppingListAggregate;

public class RecipeExistsException : DomainException
{
    public RecipeExistsException(EntityId recipeId) : base($"Recipe with id {recipeId} already exists.")
    {
    }
}