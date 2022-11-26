using System.Globalization;
using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.Enums;
using Recipes.Domain.Exceptions;
using Recipes.Domain.Extensions;
using Recipes.Shared;

namespace Recipes.Domain.ValueObjects;

public sealed class Quantity : ValueObject<Quantity>
{
    public const double ComparisonEpsilon = 1e-8;

    public Quantity(double value, QuantityUnit unit)
    {
        Value = Guard.Against.NegativeOrInvalid(value);
        Unit = Guard.Against.EnumOutOfRange(unit);
    }

    public double Value { get; }
    public QuantityUnit Unit { get; }

    public Quantity Empty => new(0, Unit);

    public Quantity ImplicitlyConvertTo(QuantityUnit unit)
    {
        if (unit == Unit)
        {
            return this;
        }

        var grams = Unit.TryGetGrams();
        if (grams.HasValue)
        {
            return new Quantity(unit.FromGrams(grams.Value) * Value, unit);
        }

        var milliliters = Unit.TryGetMilliliters();
        if (milliliters.HasValue)
        {
            return new Quantity(unit.FromMilliliters(milliliters.Value) * Value, unit);
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
        var grams = Unit.TryGetGrams();
        if (grams.HasValue)
        {
            return grams.Value * Value;
        }

        var milliliters = Unit.TryGetMilliliters();
        if (milliliters.HasValue)
        {
            return milliliters.Value * Value;
        }

        throw new QuantityUnitNonConvertibleException(Unit);
    }

    public override string ToString()
    {
        return Unit switch
        {
            QuantityUnit.Grams => $"{Value} г",
            QuantityUnit.Milliliters => $"{Value} мл",
            QuantityUnit.Pieces => $"{Value} шт",
            QuantityUnit.TeaSpoons => $"{Value} ч.л",
            QuantityUnit.TableSpoons => $"{Value} ст.л",
            QuantityUnit.DessertSpoons => $"{Value} дс.л",
            QuantityUnit.Cups => $"{Value} ст",
            QuantityUnit.Kilograms => $"{Value} кг",
            QuantityUnit.Decilitres => $"{Value} дл",
            QuantityUnit.Liters => $"{Value} л",
            _ => throw new ArgumentOutOfRangeException()
        };
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

    public bool IsImplicitlyConvertibleTo(QuantityUnit unit) => Unit.IsImplicitlyConvertibleTo(unit);

    public override int GetHashCode() => HashCode.Combine(Value, Unit);

    public bool LessThanWithRatio(Quantity quantity, double ratio)
    {
        if (!IsImplicitlyConvertibleTo(quantity.Unit))
        {
            throw new QuantityIncomparableException(this, quantity);
        }

        var elementaryUnit = ToElementaryUnit();
        var otherElementaryUnit = quantity.ToElementaryUnit();

        return elementaryUnit < otherElementaryUnit * (1 + ratio);
    }

    public static Quantity? TryParse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var split = value.Split();
        var unit = split[1].Trim() switch
        {
            "г" => QuantityUnit.Grams,
            "мл" => QuantityUnit.Milliliters,
            "шт" => QuantityUnit.Pieces,
            "ч.л" => QuantityUnit.TeaSpoons,
            "ст.л" => QuantityUnit.TableSpoons,
            "дс.л" => QuantityUnit.DessertSpoons,
            "ст" => QuantityUnit.Cups,
            "кг" => QuantityUnit.Kilograms,
            "дл" => QuantityUnit.Decilitres,
            "л" => QuantityUnit.Liters,
            _ => (QuantityUnit)(-1)
        };

        if (unit == (QuantityUnit)(-1))
        {
            return null;
        }

        if (!double.TryParse(split[0].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture,
                out var amount))
        {
            return null;
        }

        return new Quantity(amount, unit);
    }
}