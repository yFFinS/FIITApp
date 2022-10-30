using Recipes.Domain.Enums;
using Recipes.Domain.Exceptions;
using Recipes.Domain.Extensions;
using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Services;

public class QuantityConverter : IQuantityConverter
{
    private readonly Dictionary<EntityId, double> _productConversionFactors = new();

    public void SetConversionFactor(EntityId productId, double conversionFactorToElementary)
    {
        _productConversionFactors[productId] = conversionFactorToElementary;
    }

    public void RemoveConversionFactor(EntityId productId)
    {
        _productConversionFactors.Remove(productId);
    }

    public bool CanConvert(QuantityUnit fromUnit, QuantityUnit toUnit, EntityId targetProductId)
    {
        if (!IsValidConversion(fromUnit, toUnit))
        {
            return false;
        }

        return _productConversionFactors.ContainsKey(targetProductId);
    }

    public bool IsValidConversion(QuantityUnit fromUnit, QuantityUnit toUnit)
    {
        return (fromUnit.IsWeight() && toUnit == QuantityUnit.Pieces ||
                fromUnit == QuantityUnit.Pieces && toUnit.IsWeight());
    }

    public Quantity Convert(Quantity quantity, QuantityUnit targetUnit, EntityId targetProductId)
    {
        if (!CanConvert(quantity.Unit, targetUnit, targetProductId))
        {
            throw new QuantityUnitConversionException(quantity.Unit, targetUnit);
        }

        var conversionFactor = _productConversionFactors[targetProductId];

        if (targetUnit.IsWeight())
        {
            var convertedValue = quantity.Value * conversionFactor;
            return new Quantity(convertedValue, QuantityUnit.Grams).ImplicitlyConvertTo(targetUnit);
        }
        else
        {
            var convertedValue = quantity.ToElementaryUnit() / conversionFactor;
            return new Quantity(convertedValue, QuantityUnit.Pieces);
        }
    }
}