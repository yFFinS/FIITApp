using Ardalis.GuardClauses;
using Recipes.Domain.Base;

namespace Recipes.Domain.ValueObjects;

public class CookingStep : ValueObject<CookingStep>
{
    private readonly string _description;

    public string Description
    {
        get => _description;
        set => throw new NotSupportedException();
    }

    public CookingStep(string description)
    {
        _description = Guard.Against.NullOrWhiteSpace(description);
    }
    
    private CookingStep() {}
    
}