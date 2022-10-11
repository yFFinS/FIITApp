using Ardalis.GuardClauses;

namespace Recipes.Domain.ValueObjects;

public readonly struct CookingStep
{
    public readonly string Description;

    public string DescriptionProperty => Description;

    public CookingStep(string description)
    {
        Description = Guard.Against.NullOrWhiteSpace(description);
    }
}