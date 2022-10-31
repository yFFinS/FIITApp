using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.RecipeAggregate;

public class Recipe : Entity<EntityId>
{
    private string _title = null!;
    private string? _description;
    private int _servings;
    private TimeSpan _cookingTime;

    public string Title
    {
        get => _title;
        private set => _title = Guard.Against.Null(value);
    }

    public string? Description
    {
        get => _description;
        private set => _description = value;
    }

    public int Servings
    {
        get => _servings;
        private set => _servings = Guard.Against.NegativeOrZero(value);
    }

    public TimeSpan CookDuration
    {
        get => _cookingTime;
        private set => _cookingTime = Guard.Against.NegativeOrZero(value);
    }

    private readonly IngredientGroup _ingredientGroup;
    private readonly CookingTechnic _cookingTechnic;


    public Recipe(EntityId id, string title) : base(id)
    {
        Title = title;
        _ingredientGroup = new IngredientGroup();
        _cookingTechnic = new CookingTechnic();
    }

    public Recipe(EntityId id, string title, string? description, int servings, TimeSpan cookDuration) : this(id, title)
    {
        Description = description;
        Servings = servings;
        CookDuration = cookDuration;
    }

    public Recipe(EntityId id, string title, string? description, int servings, TimeSpan cookDuration,
        IngredientGroup ingredientGroup, CookingTechnic cookingTechnic) : this(id, title, description, servings,
        cookDuration)
    {
        _ingredientGroup = ingredientGroup;
        _cookingTechnic = cookingTechnic;
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
        _ingredientGroup.Update(ingredient);
    }

    public void AddIngredient(Ingredient ingredient)
    {
        ArgumentNullException.ThrowIfNull(ingredient);

        _ingredientGroup.Add(ingredient);
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        ArgumentNullException.ThrowIfNull(ingredient);

        _ingredientGroup.Remove(ingredient);
    }

    public void UpdateTitle(string title)
    {
        Title = title;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }

    public void UpdateServings(int servings)
    {
        Servings = servings;
    }

    public void UpdateCookDuration(TimeSpan cookDuration)
    {
        CookDuration = cookDuration;
    }

    public IReadOnlyCollection<CookingStep> CookingSteps => _cookingTechnic.CookingSteps;
    public IReadOnlyCollection<Ingredient> Ingredients => _ingredientGroup.AsReadOnlyCollection();
}