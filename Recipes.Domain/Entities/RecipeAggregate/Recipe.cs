using System.Reflection.Metadata;
using Ardalis.GuardClauses;
using Recipes.Domain.Base;

namespace Recipes.Domain.Entities.RecipeAggregate;

public class Recipe : BaseEntity
{
    private bool _isValid;

    private string _title;
    private string? _description;
    private int _servings;
    private TimeSpan _cookingTime;

    public string Title { get; set; }

    private T Revalidate<T>(T value)
    {   
        return value;
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

    public Ingredients Ingredients { get; set; }
    public CookingTechnic CookingTechnic { get; set; }

    public Recipe()
    {
        _title = string.Empty;
        Ingredients = new Ingredients();
        CookingTechnic = new CookingTechnic();
    }

    public Recipe(string title, string? description, int servings, TimeSpan cookDuration, Ingredients ingredients,
        CookingTechnic cookingTechnic)
    {
        Title = title;
        Description = description;
        Servings = servings;
        CookDuration = cookDuration;
        Ingredients = ingredients;
        CookingTechnic = cookingTechnic;
    }
}