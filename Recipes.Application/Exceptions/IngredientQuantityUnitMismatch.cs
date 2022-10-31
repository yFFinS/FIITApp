using Recipes.Domain.Base;
using Recipes.Domain.IngredientsAggregate;

namespace Recipes.Application.Exceptions;

public class IngredientQuantityUnitMismatch : DomainException
{
    public Ingredient NewIngredient { get; }
    public Ingredient ExistingIngredient { get; }

    public IngredientQuantityUnitMismatch(Ingredient newIngredient, Ingredient existingIngredient) : base(
        $"Ingredient quantity unit mismatch. New ingredient: {newIngredient}, existing ingredient: {existingIngredient}")
    {
        NewIngredient = newIngredient;
        ExistingIngredient = existingIngredient;
    }
}