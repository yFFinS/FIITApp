using Recipes.Domain.Enums;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Interfaces;

public interface IQuantityConverter
{
    void SetConversionFactor(EntityId productId, double conversionFactor);
    void RemoveConversionFactor(EntityId productId);
    bool CanConvert(Quantity quantity, QuantityUnit targetUnit, EntityId targetProductId);
    Quantity Convert(Quantity quantity, QuantityUnit targetUnit, EntityId targetProductId);
}