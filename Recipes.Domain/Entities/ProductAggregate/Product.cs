using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.ProductAggregate;

public sealed class Product : Entity<EntityId>
{
    private string _name = null!;
    private PriceForQuantity _priceForQuantity = null!;

    public string Name
    {
        get => _name;
        set => _name = Guard.Against.NullOrWhiteSpace(value);
    }

    public PriceForQuantity PriceForQuantity
    {
        get => _priceForQuantity;
        set => _priceForQuantity = Guard.Against.Null(value);
    }

    public Product(EntityId id, string name, PriceForQuantity priceForQuantity) : base(id)
    {
        Name = name;
        PriceForQuantity = priceForQuantity;
    }
}