using Recipes.Domain.Enums;

namespace Recipes.Domain.Extensions;

public static class QuantityUnitExtensions
{
    public static bool IsConvertibleTo(this QuantityUnit unit, QuantityUnit otherUnit)
    {
        if (unit == otherUnit)
        {
            return true;
        }

        throw new NotImplementedException();
    }

    public static bool IsConvertible(this QuantityUnit unit) => unit != QuantityUnit.Pieces;

    public static bool GetMilliliters(this QuantityUnit unit, out double milliliters)
    {
        switch (unit)
        {
            case QuantityUnit.Milliliters:
                milliliters = 1;
                return true;
            case QuantityUnit.TeaSpoons:
                milliliters = 5;
                return true;
            case QuantityUnit.TableSpoons:
                milliliters = 12;
                return true;
            case QuantityUnit.DessertSpoons:
                milliliters = 10;
                return true;
            case QuantityUnit.Cups:
                milliliters = 250;
                return true;
            case QuantityUnit.Decilitres:
                milliliters = 100;
                return true;
            case QuantityUnit.Liters:
                milliliters = 1000;
                return true;
            default:
                milliliters = 0;
                return false;
        };
    }

    public static bool GetGrams(this QuantityUnit unit, out double grams)
    {
        switch (unit)
        {
            case QuantityUnit.Grams:
                grams = 1;
                return true;
            case QuantityUnit.Kilograms:
                grams = 1000;
                return true;
            default:
                grams = 0;
                return false;
        };
    }
}