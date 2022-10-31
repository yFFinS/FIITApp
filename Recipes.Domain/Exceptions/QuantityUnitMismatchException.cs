using Recipes.Domain.Base;
using Recipes.Domain.Enums;

namespace Recipes.Domain.Exceptions;

public class QuantityUnitMismatchException : DomainException
{
    public QuantityUnitMismatchException(QuantityUnit leftUnit, QuantityUnit rightUnit) : base(
        $"Quantity units mismatch: {leftUnit} and {rightUnit}")
    {
    }
}