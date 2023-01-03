using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Interfaces;

public interface IQuantityConverter
{
    void SetConversionFactor(EntityId productId, double conversionFactorToElementary);
    void RemoveConversionFactor(EntityId productId);
    bool CanConvert(QuantityUnit fromUnit, QuantityUnit toUnit, EntityId targetProductId);
    bool IsValidConversion(QuantityUnit fromUnit, QuantityUnit toUnit);
    Quantity Convert(Quantity quantity, QuantityUnit targetUnit, EntityId targetProductId);
}