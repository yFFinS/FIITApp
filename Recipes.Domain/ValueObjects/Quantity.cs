using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.Enums;
using Recipes.Domain.Exceptions;
using Recipes.Domain.Extensions;
using Recipes.Shared;

namespace Recipes.Domain.ValueObjects;

public sealed class Quantity : ValueObject
{
    public Quantity(double value, QuantityUnit unit)
    {
        Value = Guard.Against.NegativeOrInvalid(value);
        Unit = Guard.Against.EnumOutOfRange(unit);
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

    public override string ToString()
    {
        return Unit switch
        {
            QuantityUnit.Cups => $"{Value} кр",
            QuantityUnit.Milliliters => $"{Value} мл",
            QuantityUnit.Grams => $"{Value} г",
            QuantityUnit.Pieces => $"{Value} шт",
            QuantityUnit.Teaspoons => $"{Value} ч.л",
            QuantityUnit.Tablespoons => $"{Value} ст.л",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Unit;
    }
}