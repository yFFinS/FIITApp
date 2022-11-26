using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

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
    public Uri? ImageUrl { get; set; }

    public Product(EntityId id, string name) : base(id)
    {
        Name = name;
    }
}