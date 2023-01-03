using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Interfaces;

public interface IQuantityUnitRepository
{
    IReadOnlyList<QuantityUnit> GetAllUnits();
    QuantityUnit? GetUnitById(int id);
    int GetUnitId(QuantityUnit unit);
}