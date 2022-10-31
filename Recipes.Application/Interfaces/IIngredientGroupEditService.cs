using Recipes.Domain.IngredientsAggregate;

namespace Recipes.Application.Interfaces;

public interface IIngredientGroupEditService
{
    void AddByQuantity(IngredientGroup group, Ingredient ingredient);
    Ingredient RemoveByQuantity(IngredientGroup group, Ingredient ingredient);
}