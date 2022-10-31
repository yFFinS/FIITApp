using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.ProductAggregate;

public sealed class PriceForQuantity : ValueObject<PriceForQuantity>
{
    public Price Price { get; }
    public Quantity Quantity { get; }

    public PriceForQuantity(Price price, Quantity quantity)
    {
        Price = Guard.Against.Null(price);
        Quantity = Guard.Against.Null(quantity);
    }
}