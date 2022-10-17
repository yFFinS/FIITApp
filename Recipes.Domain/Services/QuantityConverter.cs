using Recipes.Domain.Enums;
using Recipes.Domain.Exceptions;
using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Services;

public class QuantityConverter : IQuantityConverter
{
    private readonly Dictionary<EntityId, double> _productConversionFactors = new();

    public void SetConversionFactor(EntityId productId, double conversionFactor)
    {
        _productConversionFactors[productId] = conversionFactor;
    }

    public void RemoveConversionFactor(EntityId productId)
    {
        _productConversionFactors.Remove(productId);
    }

    public bool CanConvert(Quantity quantity, QuantityUnit targetUnit, EntityId targetProductId)
    {
        return _productConversionFactors.ContainsKey(targetProductId);
    }

    public Quantity Convert(Quantity quantity, QuantityUnit targetUnit, EntityId targetProductId)
    {
        if (!_productConversionFactors.TryGetValue(targetProductId, out var conversionFactor))
        {
            throw new QuantityUnitConversionException(quantity.Unit, targetUnit);
        }

        return new Quantity(quantity.Value * conversionFactor, quantity.Unit);
    }
}