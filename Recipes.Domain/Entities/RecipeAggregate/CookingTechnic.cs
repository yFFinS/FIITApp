namespace Recipes.Domain.Entities.RecipeAggregate;

public class CookingTechnic
{
    private readonly List<CookingStep> _cookingSteps;

    public CookingTechnic()
    {
        _cookingSteps = new List<CookingStep>();
    }

    public CookingTechnic(IEnumerable<CookingStep> cookingSteps)
    {
        _cookingSteps = new List<CookingStep>(cookingSteps);
    }

    public void AddCookingStep(CookingStep cookingStep)
    {
        _cookingSteps.Add(cookingStep);
    }
    
    public void RemoveCookingStep(CookingStep cookingStep)
    {
        _cookingSteps.Remove(cookingStep);
    }

    public IReadOnlyCollection<CookingStep> CookingSteps => _cookingSteps;
}