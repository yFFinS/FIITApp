using Recipes.Domain.Base;
using Recipes.Domain.Enums;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Exceptions;

public class QuantityUncomparableException : DomainException
{
    public QuantityUncomparableException(Quantity q1, Quantity q2) : base(
        $"Cannot compare {q1} and {q2}.")
    {
    }
}