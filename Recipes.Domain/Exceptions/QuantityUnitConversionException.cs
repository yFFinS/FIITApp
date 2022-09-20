using Recipes.Domain.Base;
using Recipes.Domain.Enums;

namespace Recipes.Domain.Exceptions;

public class QuantityUnitConversionException : DomainException
{
    public QuantityUnitConversionException(QuantityUnit from, QuantityUnit to) : base(
        $"Cannot convert from {from} to {to}.")
    {
    }
}