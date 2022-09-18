namespace Recipes.Domain.Entities.RecipeAggregate;

public class CookingStep
{
    public string Description { get; }

    public CookingStep(string description)
    {
        Description = description;
    }
}