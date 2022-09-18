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
}