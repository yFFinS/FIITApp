using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.IngredientsAggregate;

public class Ingredient : ValueObject<Ingredient>
{
    public EntityId ProductId { get; }
    public Quantity Quantity { get; }

    [EqualityIgnore] public Ingredient Empty => new(ProductId, Quantity.Empty);

    public Ingredient(EntityId productId, Quantity quantity)
    {
        ProductId = Guard.Against.Null(productId);
        Quantity = Guard.Against.Null(quantity);
    }

    public Ingredient WithQuantity(Quantity quantity)
    {
        return new Ingredient(ProductId, quantity);
    }

    public Ingredient WithProduct(EntityId productId)
    {
        return new Ingredient(productId, Quantity);
    }

    public override string ToString()
    {
        return $"{ProductId} {Quantity}";
    }
}