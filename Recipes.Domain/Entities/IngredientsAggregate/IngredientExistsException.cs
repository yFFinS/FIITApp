using Recipes.Domain.Base;

namespace Recipes.Domain.Entities.IngredientsAggregate;

public class IngredientExistsException : DomainException
{
    public IngredientExistsException(string ingredientName) : base($"Ingredient {ingredientName} already exists")
    {
    }
}