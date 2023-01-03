using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Exceptions;

public class QuantityUnitNonConvertibleException : DomainException
{
    public QuantityUnitNonConvertibleException(QuantityUnit unit) : base(
        $"{unit.Name} is non-convertible.")
    {
    }
}