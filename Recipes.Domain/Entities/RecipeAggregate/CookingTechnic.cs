using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.RecipeAggregate;

public class CookingTechnic
{
    private readonly List<CookingStep> _cookingSteps;

    public CookingTechnic() : this(Enumerable.Empty<CookingStep>())
    {
    }

    public CookingTechnic(IEnumerable<CookingStep> cookingSteps)
    {
        _cookingSteps = new List<CookingStep>(cookingSteps);
    }

    public void AddStep(CookingStep cookingStep)
    {
        _cookingSteps.Add(cookingStep);
    }

    public void RemoveCookingStep(CookingStep cookingStep)
    {
        _cookingSteps.Remove(cookingStep);
    }

    public IReadOnlyCollection<CookingStep> CookingSteps => _cookingSteps;

    public void SetCookingStep(int setIndex, CookingStep cookingStep)
    {
        _cookingSteps[setIndex] = cookingStep;
    }

    public void InsertCookingStep(int insertIndex, CookingStep cookingStep)
    {
        _cookingSteps.Insert(insertIndex, cookingStep);
    }
}