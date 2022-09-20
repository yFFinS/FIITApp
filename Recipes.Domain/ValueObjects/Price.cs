using Ardalis.GuardClauses;
using Recipes.Domain.Base;

namespace Recipes.Domain.ValueObjects;

public class Price : ValueObject
{
    public readonly decimal Value;


    // TODO: add currency
    public Price(decimal value)
    {
        Value = Guard.Against.Negative(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}