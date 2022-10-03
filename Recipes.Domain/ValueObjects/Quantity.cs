using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.Enums;
using Recipes.Domain.Exceptions;
using Recipes.Domain.Extensions;
using Recipes.Domain.Interfaces;
using Recipes.Shared;

namespace Recipes.Domain.ValueObjects;

public sealed class Quantity : ValueObject
{
    public Quantity(double value, QuantityUnit unit, double? density = null)
    {
        Value = Guard.Against.NegativeOrInvalid(value);
        Unit = Guard.Against.EnumOutOfRange(unit);
        if (density != null)
        {
            this.density = Guard.Against.NegativeOrZero((double)density);
        }
        SetGramsAndMiilliliters(); 
    }

    public readonly double Value;
    public readonly QuantityUnit Unit;

    private double? density;
    private double? grams;
    private double? milliliters;

    private void SetGramsAndMiilliliters()
    {
        var inGrams = Unit.GetGrams();
        var inMilliliters = Unit.GetMilliliters();
        if (inGrams == -1 && inMilliliters != -1)
        {
            if (density != null)
            {
                grams = Value * inMilliliters * density;
            }
            milliliters = Value * inMilliliters;
        }
        else if (inGrams != -1 && inMilliliters == -1)
        {
            grams = Value * inGrams;
            if (density != null)
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
        if (!unit.IsConvertible())
        {
            throw new QuantityUnitNonConvertibleException(unit);
        }

        var newGrams = unit.GetGrams();
        if (grams != null && newGrams != -1)
        {
            return new Quantity((double)grams / newGrams, unit, density);
        }

        var newMilliliters = unit.GetMilliliters();
        if (milliliters != null && newMilliliters != -1)
        {
            return new Quantity((double)milliliters / newMilliliters, unit, density);
        }

        throw new QuantityUnitConversionException(Unit, unit);
    }

    public static bool operator ==(Quantity q1, Quantity q2)
    {
        if (ReferenceEquals(q1, q2))
        {
            return true;
        }
        if (q1.milliliters != null && q2.milliliters != null)
        {
            return q1.milliliters.Equals(q2.milliliters);
        }
        if (q1.grams != null && q2.grams != null)
        {
            return q1.grams.Equals(q2.grams);
        }
        throw new QuantityUncomparableException(q1, q2);
    }

    public static bool operator !=(Quantity q1, Quantity q2)
    {
        return !(q1 == q2);
    }

    public static bool operator <(Quantity q1, Quantity q2)
    {
        if (q1.milliliters != null && q2.milliliters != null)
        {
            return q1.milliliters <= q2.milliliters;
        }
        if (q1.grams != null && q2.grams != null)
        {
            return q1.grams <= q2.grams;
        }
        throw new QuantityUncomparableException(q1, q2);
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
        if (ReferenceEquals(q, null))
        {
            return false;
        }

        if (!(q is Quantity))
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