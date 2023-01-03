using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Recipes.Domain.Entities.ProductAggregate;

public sealed class Product : Entity<EntityId>
{
    private string _name = null!;

    public string Name
    {
        get => _name;
        set => _name = Guard.Against.NullOrWhiteSpace(value);
    }

    public string? Description { get; set; }

    private readonly List<QuantityUnit> _quantityUnits = new();

    public IReadOnlyList<QuantityUnit> ValidQuantityUnits => _quantityUnits;

    public double? PieceWeight { get; set; }

    [XmlIgnore] public Uri? ImageUrl { get; set; }

    [XmlElement("Uri")]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public string? ImageUrlString
    {
        get => ImageUrl?.ToString();
        set => ImageUrl = value == null ? null : new Uri(value);
    }

    public Product(EntityId id, string name) : base(id)
    {
        Name = name;
        _quantityUnits = new List<QuantityUnit>();
    }

    public Product(EntityId id, string name, string description, Uri? imageUrl,
        IEnumerable<QuantityUnit> validQuantityUnits) : base(id)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        _quantityUnits = validQuantityUnits.ToList();
    }

    private Product()
    {
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