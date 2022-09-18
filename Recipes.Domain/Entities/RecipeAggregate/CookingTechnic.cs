namespace Recipes.Domain.Entities.RecipeAggregate;

public class CookingTechnic
{
    private readonly List<CookingStep> _cookingSteps;

    public CookingTechnic(IEnumerable<CookingStep> cookingSteps)
    {
        _cookingSteps = new List<CookingStep>(cookingSteps);
    }

    public void AddCookingStep(CookingStep cookingStep)
    {
        _cookingSteps.Add(cookingStep);
    }

    public IReadOnlyCollection<CookingStep> CookingSteps => _cookingSteps;
}