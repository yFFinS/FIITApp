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

    public readonly double Value;
    public readonly QuantityUnit Unit;

    private double? density;
    private double? grams;
    private double? milliliters;

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
}