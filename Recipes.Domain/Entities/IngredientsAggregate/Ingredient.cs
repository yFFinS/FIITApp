namespace Recipes.Domain.Entities.IngredientsAggregate;

public class Ingredient
{
    public string Name { get; }
    public string Description { get; }
    public string? ImageUrl { get; }

    public Ingredient(string name, string description, string? imageUrl = null)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
    }
}