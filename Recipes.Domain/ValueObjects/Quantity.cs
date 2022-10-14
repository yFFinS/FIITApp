using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.Enums;
using Recipes.Domain.Exceptions;
using Recipes.Domain.Extensions;
using Recipes.Shared;

namespace Recipes.Domain.ValueObjects;

public sealed class Quantity : ValueObject
{
    public Quantity(double value, QuantityUnit unit, double? density = null)
    {
        Value = Guard.Against.NegativeOrInvalid(value);
        Unit = Guard.Against.EnumOutOfRange(unit);
        if (density is not null)
        {
            this.density = Guard.Against.NegativeOrZero((double)density);
        }
        SetGramsAndMiilliliters();
    }

    public readonly double Value;
    public readonly QuantityUnit Unit;
    public Quantity Empty => new Quantity(0, Unit);

    private readonly double? density;
    private double? grams;
    private double? milliliters;

    private void SetGramsAndMiilliliters()
    {
        var gotGrams = Unit.GetGrams(out var inGrams);
        var gotMilliliters = Unit.GetMilliliters(out var inMilliliters);
        if (!gotGrams && gotMilliliters)
        {
            if (density is not null)
            {
                grams = Value * inMilliliters * density;
            }
            milliliters = Value * inMilliliters;
        }
        else if (gotGrams && !gotMilliliters)
        {
            grams = Value * inGrams;
            if (density is not null)
            {
                milliliters = Value * inGrams / density;
            }
        }
    }

    public Quantity ConvertTo(QuantityUnit unit)
    {
        if (!Unit.IsConvertible())
        {
            throw new QuantityUnitNonConvertibleException(Unit);
        }

        var gotGrams = unit.GetGrams(out var newGrams);
        if (grams is not null && gotGrams)
        {
            return new Quantity(Math.Round((double)grams / newGrams * 10000) / 10000,
                unit, density);
        }

        var gotMilliliters = unit.GetMilliliters(out var newMilliliters);
        if (milliliters is not null && gotMilliliters)
        {
            return new Quantity(Math.Round((double)milliliters / newMilliliters * 10000) / 10000,
                unit, density);
        }

        throw new QuantityUnitConversionException(Unit, unit);
    }

    public static bool operator ==(Quantity q1, Quantity q2)
    {
        if (ReferenceEquals(q1, q2))
        {
            return true;
        }
        if (q1.milliliters is not null && q2.milliliters is not null)
        {
            return Math.Abs((double)q1.milliliters - (double)q2.milliliters) < 0.0001;
        }
        if (q1.grams is not null && q2.grams is not null)
        {
            return Math.Abs((double)q1.grams - (double)q2.grams) < 0.0001;
        }
        throw new QuantityUncomparableException(q1, q2);
    }

    public static bool operator !=(Quantity q1, Quantity q2)
    {
        return !(q1 == q2);
    }

    public static bool operator <(Quantity q1, Quantity q2)
    {
        if (q1.milliliters is not null && q2.milliliters is not null)
        {
            return q1.milliliters <= q2.milliliters;
        }
        if (q1.grams is not null && q2.grams is not null)
        {
            return q1.grams <= q2.grams;
        }
        throw new QuantityUncomparableException(q1, q2);
    }

    public bool LessThanWithMargin(Quantity q, double margin)
    {
        margin = Guard.Against.NegativeOrZero(margin) + 1;
        if (milliliters is not null && q.milliliters is not null)
        {
            return milliliters <= q.milliliters * margin;
        }
        if (grams is not null && q.grams is not null)
        {
            return grams <= q.grams * margin;
        }
        throw new QuantityUncomparableException(this, q);
    }

    public bool GreaterThanWithMargin(Quantity q, double margin)
    {
        margin = Guard.Against.NegativeOrZero(margin) + 1;
        if (q.milliliters is not null && q.milliliters is not null)
        {
            return q.milliliters * margin >= q.milliliters;
        }
        if (q.grams is not null && q.grams is not null)
        {
            return q.grams * margin >= q.grams;
        }
        throw new QuantityUncomparableException(this, q);
    }

    public static bool operator >(Quantity q1, Quantity q2)
    {
        return !(q1 < q2);
    }

    public static bool operator <=(Quantity q1, Quantity q2)
    {
        return q1 == q2 || q1 < q2;
    }

    public static bool operator >=(Quantity q1, Quantity q2)
    {
        return q1 == q2 || q1 > q2;
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

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Unit;
    }

    public override bool Equals(object? q)
    {
        if (q is null)
        {
            return false;
        }

        if (q is not Quantity)
        {
            return false;
        }

        if (ReferenceEquals(this, q))
        {
            return true;
        }

        var q1 = (Quantity)q;
        return milliliters.Equals(q1.milliliters) &&
               grams.Equals(q1.grams);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}