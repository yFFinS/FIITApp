using System.Globalization;
using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.Exceptions;
using Recipes.Shared;

namespace Recipes.Domain.ValueObjects;

public sealed class Quantity : ValueObject<Quantity>
{
    public double Value { get; }

    public QuantityUnit Unit { get; }

    public const double ComparisonEpsilon = 1e-8;

    public Quantity(double value, QuantityUnit unit)
    {
        Value = Guard.Against.NegativeOrInvalid(value);
        Unit = Guard.Against.Null(unit);
    }

    [EqualityIgnore] public Quantity Empty => new(0, Unit);

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Quantity other && Equals(other);
    }

    public Quantity ImplicitlyConvertTo(QuantityUnit unit)
    {
        if (unit == Unit)
        {
            return this;
        }

        if (Unit.GramsConversionFactor.HasValue)
        {
            return new Quantity(Unit.GramsConversionFactor.Value * Value, unit);
        }

        if (Unit.MillilitersConversionFactor.HasValue)
        {
            return new Quantity(Unit.MillilitersConversionFactor.Value * Value, unit);
        }

        throw new QuantityUnitNonConvertibleException(Unit);
    }

    public static bool operator ==(Quantity q1, Quantity q2)
    {
        if (ReferenceEquals(q1, q2))
        {
            return true;
        }

        if (ReferenceEquals(q1, null) || ReferenceEquals(q2, null))
        {
            return false;
        }

        return q1.Equals(q2);
    }

    public static bool operator !=(Quantity q1, Quantity q2)
    {
        return !(q1 == q2);
    }

    public static bool operator <(Quantity q1, Quantity q2)
    {
        if (q1.Unit == q2.Unit)
        {
            return q1.Value < q2.Value;
        }

        throw new QuantityIncomparableException(q1, q2);
    }

    public static bool operator >(Quantity q1, Quantity q2)
    {
        if (q1.Unit == q2.Unit)
        {
            return q1.Value > q2.Value;
        }

        throw new QuantityIncomparableException(q1, q2);
    }

    public static bool operator <=(Quantity q1, Quantity q2)
    {
        return q1 == q2 || q1 < q2;
    }

    public static bool operator >=(Quantity q1, Quantity q2)
    {
        return q1 == q2 || q1 > q2;
    }

    public static Quantity operator +(Quantity q1, Quantity q2)
    {
        if (q1.Unit == q2.Unit)
        {
            return new Quantity(q1.Value + q2.Value, q1.Unit);
        }

        throw new QuantityUnitMismatchException(q1.Unit, q2.Unit);
    }

    public static Quantity operator -(Quantity q1, Quantity q2)
    {
        if (q1.Unit == q2.Unit)
        {
            return new Quantity(q1.Value - q2.Value, q1.Unit);
        }

        throw new QuantityUnitMismatchException(q1.Unit, q2.Unit);
    }

    public double ToElementaryUnit()
    {
        if (Unit.GramsConversionFactor.HasValue)
        {
            return Unit.GramsConversionFactor.Value * Value;
        }

        if (Unit.GramsConversionFactor.HasValue)
        {
            return Unit.GramsConversionFactor.Value * Value;
        }

        throw new QuantityUnitNonConvertibleException(Unit);
    }

    public override string ToString()
    {
        var unitString = Unit.GetAbbreviation(Value);
        return !Unit.IsMeasurable ? unitString : $"{Value.ToString(CultureInfo.InvariantCulture)} {unitString}";
    }

    public override bool Equals(Quantity? quantity)
    {
        if (quantity is null)
        {
            return false;
        }

        if (ReferenceEquals(this, quantity))
        {
            return true;
        }

        return Math.Abs(Value - quantity.Value) < ComparisonEpsilon && Unit == quantity.Unit;
    }

    public override int GetHashCode() => HashCode.Combine(Value, Unit);

    public bool LessThanWithRatio(Quantity quantity, double ratio)
    {
        var elementaryUnit = ToElementaryUnit();
        var otherElementaryUnit = quantity.ToElementaryUnit();

        return elementaryUnit < otherElementaryUnit * (1 + ratio);
    }
}