using Ardalis.GuardClauses;
using Recipes.Domain.Base;

namespace Recipes.Domain.ValueObjects;

public class Price : ValueObject<Price>
{
    public decimal Value { get; }

    public Price(decimal value)
    {
        Value = Guard.Against.Negative(value);
    }
}