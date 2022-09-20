using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.ProductAggregate;

public sealed class PriceForQuantity : ValueObject
{
    public readonly Price Price;
    public readonly Quantity Quantity;

    public PriceForQuantity(Price price, Quantity quantity)
    {
        Price = Guard.Against.Null(price);
        Quantity = Guard.Against.Null(quantity);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Price;
        yield return Quantity;
    }
}