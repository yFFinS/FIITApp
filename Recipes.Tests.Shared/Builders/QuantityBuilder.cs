using Recipes.Domain.ValueObjects;

namespace Recipes.Tests.Shared.Builders;

public class QuantityBuilder : AbstractBuilder<Quantity>
{
    private QuantityUnit _unit = new("штуки", "шт");
    private double _value = 1;

    public QuantityBuilder WithUnit(QuantityUnit unit)
    {
        _unit = unit;
        return this;
    }

    public QuantityBuilder WithValue(double value)
    {
        _value = value;
        return this;
    }

    public override Quantity Build()
    {
        return new Quantity(_value, _unit);
    }
}