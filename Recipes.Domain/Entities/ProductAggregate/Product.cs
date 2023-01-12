using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.ProductAggregate;

public sealed class Product : Entity<EntityId>
{
    public string Name
    {
        get => _name;
        set => _name = Guard.Against.NullOrWhiteSpace(value);
    }

    public string? Description { get; set; }

    public double? PieceWeight { get; set; }

    public Uri? ImageUrl { get; set; }

    public IReadOnlyList<QuantityUnit> ValidQuantityUnits => _quantityUnits;
    private string _name = null!;

    private readonly List<QuantityUnit> _quantityUnits;

    public Product(EntityId id, string name) : base(id)
    {
        Name = name;
        _quantityUnits = new List<QuantityUnit>();
    }

    public Product(EntityId id, string name, string? description,
        IEnumerable<QuantityUnit> validQuantityUnits) : base(id)
    {
        Name = name;
        Description = description;
        _quantityUnits = validQuantityUnits.ToList();
    }

    public void AddQuantityUnit(QuantityUnit quantityUnit)
    {
        _quantityUnits.Add(quantityUnit);
    }

    public bool IsAvailableQuantityUnit(QuantityUnit quantityUnit)
    {
        return _quantityUnits.Contains(quantityUnit);
    }
}