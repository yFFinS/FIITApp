using Recipes.Domain.Base;
using Recipes.Domain.Enums;

namespace Recipes.Domain.Exceptions;

public class QuantityUnitNonConvertibleException : DomainException
{
    public QuantityUnitNonConvertibleException(QuantityUnit unit) : base(
        $"{unit} is non-convertible.")
    {
    }
}