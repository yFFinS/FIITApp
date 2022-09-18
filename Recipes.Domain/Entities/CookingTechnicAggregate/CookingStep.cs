namespace Recipes.Domain.Entities.CookingTechnicAggregate;

public class CookingStep
{
    public string Description { get; }

    public CookingStep(string description)
    {
        Description = description;
    }
}