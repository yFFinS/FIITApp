using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Exceptions;

public class QuantityUnitMismatchException : DomainException
{
    public QuantityUnitMismatchException(QuantityUnit leftUnit, QuantityUnit rightUnit) : base(
        $"Quantity units mismatch: {leftUnit.Names} and {rightUnit.Names}")
    {
    }
}