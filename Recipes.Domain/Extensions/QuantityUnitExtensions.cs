using Recipes.Domain.Enums;
using Recipes.Domain.Exceptions;

namespace Recipes.Domain.Extensions;

public static class QuantityUnitConversionConstants
{
    public const double TeaSpoonsToMilliliters = 5.0;
    public const double DessertSpoonsToMilliliters = 10.0;
    public const double TableSpoonsToMilliliters = 12.0;
    public const double DecilitresToMilliliters = 100.0;
    public const double CupsToMilliliters = 250.0;
    public const double LitresToMilliliters = 1000.0;

    public const double KilogramsToGrams = 1000.0;
}

public static class QuantityUnitExtensions
{
    public static bool IsWeight(this QuantityUnit unit) => unit.TryGetGrams().HasValue;

    public static bool IsVolume(this QuantityUnit unit) => unit.TryGetMilliliters().HasValue;

    public static bool IsImplicitlyConvertibleTo(this QuantityUnit unit, QuantityUnit otherUnit)
    {
        if (unit == otherUnit)
        {
            return true;
        }

        return unit.IsWeight() && otherUnit.IsWeight() || unit.IsVolume() && otherUnit.IsVolume();
    }

    public static double? TryGetMilliliters(this QuantityUnit unit)
    {
        return unit switch
        {
            QuantityUnit.Milliliters => 1.0,
            QuantityUnit.TeaSpoons => QuantityUnitConversionConstants.TeaSpoonsToMilliliters,
            QuantityUnit.TableSpoons => QuantityUnitConversionConstants.TableSpoonsToMilliliters,
            QuantityUnit.DessertSpoons => QuantityUnitConversionConstants.DessertSpoonsToMilliliters,
            QuantityUnit.Cups => QuantityUnitConversionConstants.CupsToMilliliters,
            QuantityUnit.Decilitres => QuantityUnitConversionConstants.DecilitresToMilliliters,
            QuantityUnit.Liters => QuantityUnitConversionConstants.LitresToMilliliters,
            _ => null
        };
    }

    public static double? TryGetGrams(this QuantityUnit unit)
    {
        return unit switch
        {
            QuantityUnit.Grams => 1.0,
            QuantityUnit.Kilograms => QuantityUnitConversionConstants.KilogramsToGrams,
            _ => null
        };
    }

    public static double FromGrams(this QuantityUnit unit, double grams)
    {
        var conversionFactor = unit.TryGetGrams();
        if (conversionFactor.HasValue)
        {
            return grams / conversionFactor.Value;
        }

        throw new QuantityUnitConversionException(unit, QuantityUnit.Grams);
    }

    public static double FromMilliliters(this QuantityUnit unit, double milliliters)
    {
        var conversionFactor = unit.TryGetMilliliters();
        if (conversionFactor.HasValue)
        {
            return milliliters / conversionFactor.Value;
        }

        throw new QuantityUnitConversionException(unit, QuantityUnit.Milliliters);
    }
}