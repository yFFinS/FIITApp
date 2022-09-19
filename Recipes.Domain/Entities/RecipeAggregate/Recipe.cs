using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.Entities.IngredientAggregate;
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

    public Recipe(EntityId id, string title, string? description, int servings, TimeSpan cookDuration,
        Ingredients ingredients, CookingTechnic cookingTechnic) : this(id, title, description, servings, cookDuration)
    {
        _ingredients = ingredients;
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

    public IReadOnlyCollection<CookingStep> GetCookingSteps() => _cookingTechnic.CookingSteps;
    public IReadOnlyCollection<Ingredient> GetIngredients() => _ingredients.AsReadOnlyCollection();
}