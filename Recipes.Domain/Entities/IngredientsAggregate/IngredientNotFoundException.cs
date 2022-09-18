using Recipes.Domain.Base;

namespace Recipes.Domain.Entities.IngredientsAggregate;

public class IngredientNotFoundException : DomainException
{
    public IngredientNotFoundException(string ingredientName) : base($"Ingredient {ingredientName} not found")
    {
    }
}