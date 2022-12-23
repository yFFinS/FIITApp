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
    }

    private Product()
    {
    }
}