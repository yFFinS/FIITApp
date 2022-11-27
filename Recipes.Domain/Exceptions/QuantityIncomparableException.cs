using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Exceptions;

public class QuantityIncomparableException : DomainException
{
    public QuantityIncomparableException(Quantity q1, Quantity q2) : base(
        $"Cannot compare {q1} and {q2}.")
    {
    }
}