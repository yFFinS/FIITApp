using Ardalis.GuardClauses;
using Recipes.Domain.Enums;
using Recipes.Domain.Exceptions;
using Recipes.Domain.Extensions;

namespace Recipes.Domain.ValueObjects;

public class Quantity
{
    public Quantity(double value, QuantityUnit unit)
    {
        Guard.Against.NegativeOrZero(value);

        Value = value;
        Unit = unit;
    }

    public readonly double Value;
    public readonly QuantityUnit Unit;

    public bool IsConvertibleTo(QuantityUnit unit) => Unit.IsConvertibleTo(unit);

    public Quantity ConvertTo(QuantityUnit unit)
    {
        if (!IsConvertibleTo(unit))
        {
            throw new QuantityUnitConversionException(Unit, unit);
        }

        throw new NotImplementedException();
    }
}