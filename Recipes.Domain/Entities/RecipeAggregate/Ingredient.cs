using Ardalis.GuardClauses;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.RecipeAggregate;

public class Ingredient
{
    public string Name { get; }
    public Quantity Quantity { get; }

    public Ingredient(string name, Quantity quantity)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.Null(quantity);

        Name = name;
        Quantity = quantity;
    }
}