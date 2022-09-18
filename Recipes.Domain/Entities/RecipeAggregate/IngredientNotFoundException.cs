using Recipes.Domain.Base;

namespace Recipes.Domain.Entities.RecipeAggregate;

public class IngredientNotFoundException : DomainException
{
    public IngredientNotFoundException(string ingredientName) : base($"Ingredient {ingredientName} not found")
    {
    }
}