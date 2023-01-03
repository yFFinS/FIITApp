using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Interfaces;

public interface IQuantityUnitRepository
{
    IReadOnlyList<QuantityUnit> GetAllUnits();
}