using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.Enums;
using Recipes.Domain.Exceptions;
using Recipes.Domain.Extensions;
using Recipes.Shared;

namespace Recipes.Domain.ValueObjects;

public sealed class Quantity : ValueObject, IComparable<Quantity>
{
    public Quantity(double value, QuantityUnit unit)
    {
        Value = Guard.Against.NegativeOrInvalid(value);
        Unit = Guard.Against.EnumOutOfRange(unit);
    }

    public readonly double Value;
    public readonly QuantityUnit Unit;
    public Quantity Empty => new Quantity(0, Unit);

    public bool IsConvertibleTo(QuantityUnit unit) => Unit.IsConvertibleTo(unit);

    public Quantity ConvertTo(QuantityUnit unit)
    {
        if (!IsConvertibleTo(unit))
        {
            throw new QuantityUnitConversionException(Unit, unit);
        }

        if (Unit == unit)
        {
            return this;
        }

        throw new NotImplementedException();
    }

    public static Quantity operator +(Quantity left, Quantity right)
    {
        if (left.Unit != right.Unit)
        {
            throw new QuantityUnitMismatchException(left.Unit, right.Unit);
        }

        return new Quantity(left.Value + right.Value, left.Unit);
    }


    public static Quantity operator -(Quantity left, Quantity right)
    {
        if (left.Unit != right.Unit)
        {
            throw new QuantityUnitMismatchException(left.Unit, right.Unit);
        }

        return new Quantity(left.Value - right.Value, left.Unit);
    }

    public static bool operator >(Quantity left, Quantity right)
    {
        if (left.Unit != right.Unit)
        {
            throw new QuantityUnitMismatchException(left.Unit, right.Unit);
        }

        return left.Value > right.Value;
    }

    public static bool operator <(Quantity left, Quantity right)
    {
        if (left.Unit != right.Unit)
        {
            throw new QuantityUnitMismatchException(left.Unit, right.Unit);
        }

        return left.Value < right.Value;
    }

    public static bool operator >=(Quantity left, Quantity right)
    {
        if (left.Unit != right.Unit)
        {
            throw new QuantityUnitMismatchException(left.Unit, right.Unit);
        }

        return left.Value >= right.Value;
    }

    public static bool operator <=(Quantity left, Quantity right)
    {
        if (left.Unit != right.Unit)
        {
            throw new QuantityUnitMismatchException(left.Unit, right.Unit);
        }

        return left.Value <= right.Value;
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

    public int CompareTo(Quantity? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (ReferenceEquals(null, other))
        {
            return 1;
        }

        var valueComparison = Value.CompareTo(other.Value);
        return valueComparison != 0 ? valueComparison : Unit.CompareTo(other.Unit);
    }
}