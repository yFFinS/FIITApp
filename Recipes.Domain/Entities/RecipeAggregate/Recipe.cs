using Ardalis.GuardClauses;

namespace Recipes.Domain.Entities.RecipeAggregate;

public class Recipe
{
    public string Title { get; }
    public string? Description { get; }
    public int Servings { get; }
    public TimeSpan CookDuration { get; }
    public Ingredients Ingredients { get; }
    public CookingTechnic CookingTechnic { get; }

    public Recipe(string title, string? description, int servings, TimeSpan cookDuration, Ingredients ingredients,
        CookingTechnic cookingTechnic)
    {
        Guard.Against.NullOrWhiteSpace(title);
        Guard.Against.NegativeOrZero(servings);
        Guard.Against.NegativeOrZero(cookDuration);
        Guard.Against.Null(ingredients);
        Guard.Against.Null(cookingTechnic);

        Title = title;
        Description = description;
        Servings = servings;
        CookDuration = cookDuration;
        Ingredients = ingredients;
        CookingTechnic = cookingTechnic;
    }
}