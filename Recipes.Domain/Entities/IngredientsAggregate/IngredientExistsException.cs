using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.IngredientAggregate;

public class IngredientExistsException : DomainException
{
    public IngredientExistsException(EntityId ingredientId) : base($"Ingredient with id {ingredientId} already exists")
    {
    }
}