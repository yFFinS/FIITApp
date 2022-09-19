using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.IngredientAggregate;

public class IngredientNotFoundException : DomainException
{
    public IngredientNotFoundException(EntityId ingredientId) : base($"Ingredient with id {ingredientId} not found")
    {
    }
}