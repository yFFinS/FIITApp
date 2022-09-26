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

    public static double GetMilliliters(this QuantityUnit unit)
    {
        return unit switch
        {
            QuantityUnit.Milliliters => 1,
            QuantityUnit.TeaSpoons => 5,
            QuantityUnit.TableSpoons => 12,
            QuantityUnit.DessertSpoons => 10,
            QuantityUnit.Cups => 250,
            QuantityUnit.Decilitres => 100,
            QuantityUnit.Liters => 1000,
            _ => -1
        };
    }

    public static double GetGrams(this QuantityUnit unit)
    {
        return unit switch
        {
            QuantityUnit.Grams => 1,
            QuantityUnit.Kilograms => 1000,
            _ => -1
        };
    }
}