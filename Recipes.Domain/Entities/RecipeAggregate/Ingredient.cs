using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.RecipeAggregate;

public class Ingredient : BaseEntity
{
    private string _name = null!;
    private Quantity _quantity = null!;

    public string Name
    {
        get => _name;
        set => _name = Guard.Against.NullOrWhiteSpace(value);
    }

    public Quantity Quantity
    {
        get => _quantity;
        set => _quantity = value;
    }

    public Ingredient(EntityId id, string name, Quantity quantity) : base(id)
    {
        Name = name;
        Quantity = quantity;
    }
}