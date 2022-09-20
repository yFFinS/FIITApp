using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.IngredientsAggregate;

public class IngredientNotFoundException : DomainException
{
    public IngredientNotFoundException(EntityId productId) : base($"Ingredient with product id {productId} not found")
    {
    }
}