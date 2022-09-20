using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.IngredientsAggregate;

public class IngredientExistsException : DomainException
{
    public IngredientExistsException(EntityId productId) : base($"Ingredient with product id {productId} already exists")
    {
    }
}