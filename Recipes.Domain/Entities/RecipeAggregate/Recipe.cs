using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.RecipeAggregate;

public class Recipe : BaseEntity
{
    private string _title = null!;
    private string? _description;
    private int _servings;
    private TimeSpan _cookingTime;

    public string Title
    {
        get => _title;
        set => _title = Guard.Against.Null(value);
    }

    public string? Description
    {
        get => _description;
        set => _description = value;
    }

    public int Servings
    {
        get => _servings;
        set => _servings = Guard.Against.NegativeOrZero(value);
    }

    public TimeSpan CookDuration
    {
        get => _cookingTime;
        set => _cookingTime = Guard.Against.NegativeOrZero(value);
    }

    private readonly Ingredients _ingredients;
    private readonly CookingTechnic _cookingTechnic;


    public Recipe(EntityId id, string title) : base(id)
    {
        Title = title;
        _ingredients = new Ingredients();
        _cookingTechnic = new CookingTechnic();
    }

    public Recipe(EntityId id, string title, string? description, int servings, TimeSpan cookDuration) : this(id, title)
    {
        Description = description;
        Servings = servings;
        CookDuration = cookDuration;
    }

    public void AddCookingStep(CookingStep cookingStep)
    {
        _cookingTechnic.AddStep(cookingStep);
    }

    public void RemoveCookingStep(CookingStep cookingStep)
    {
        _cookingTechnic.RemoveCookingStep(cookingStep);
    }

    public void SetCookingStep(int setIndex, CookingStep cookingStep)
    {
        _cookingTechnic.SetCookingStep(setIndex, cookingStep);
    }

    public void InsertCookingStep(int insertIndex, CookingStep cookingStep)
    {
        _cookingTechnic.InsertCookingStep(insertIndex, cookingStep);
    }

    public void UpdateIngredient(Ingredient ingredient)
    {
        ArgumentNullException.ThrowIfNull(ingredient);
        _ingredients.Update(ingredient);
    }

    public void AddIngredient(Ingredient ingredient)
    {
        ArgumentNullException.ThrowIfNull(ingredient);
        _ingredients.Add(ingredient);
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        ArgumentNullException.ThrowIfNull(ingredient);
        _ingredients.Remove(ingredient.Id);
    }

    public IReadOnlyCollection<CookingStep> CookingSteps => _cookingTechnic.CookingSteps;
    public IReadOnlyCollection<Ingredient> Ingredients => _ingredients.AsReadOnlyCollection();
}