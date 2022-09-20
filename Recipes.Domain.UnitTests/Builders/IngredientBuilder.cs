using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Enums;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.UnitTests.Builders;

public class IngredientBuilder
{
    private EntityId _id = EntityId.New();
    private string _name = "Ingredient";
    private Quantity _quantity = new Quantity(1, QuantityUnit.Pieces);

    public IngredientBuilder WithId(EntityId id)
    {
        _id = id;
        return this;
    }

    public IngredientBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public IngredientBuilder WithQuantity(Quantity quantity)
    {
        _quantity = quantity;
        return this;
    }

    public Ingredient Build()
    {
        return new Ingredient(_id, _name, _quantity);
    }

    public static implicit operator Ingredient(IngredientBuilder builder) => builder.Build();
}