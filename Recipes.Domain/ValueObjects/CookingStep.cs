using Ardalis.GuardClauses;
using Recipes.Domain.Base;

namespace Recipes.Domain.ValueObjects;

public class CookingStep : ValueObject<CookingStep>
{
    public string Description { get; }

    public CookingStep(string description)
    {
        Description = Guard.Against.NullOrWhiteSpace(description);
    }
}