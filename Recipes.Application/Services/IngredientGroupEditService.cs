using Microsoft.Extensions.Logging;
using Recipes.Application.Exceptions;
using Recipes.Application.Interfaces;
using Recipes.Domain.IngredientsAggregate;

namespace Recipes.Application.Services;

public class IngredientGroupEditService : IIngredientGroupEditService
{
    private readonly ILogger<IngredientGroupEditService> _logger;

    public IngredientGroupEditService(ILogger<IngredientGroupEditService> logger)
    {
        _logger = logger;
    }

    public void AddByQuantity(IngredientGroup group, Ingredient ingredient)
    {
        var existingIngredient = group.TryGetByProductId(ingredient.Product.Id);
        if (existingIngredient is null)
        {
            group.Add(ingredient);

            _logger.LogDebug("Added ingredient {Ingredient}", ingredient);
            return;
        }

        ThrowIfNotConvertible(ingredient, existingIngredient);

        var convertedIngredient = ingredient.WithQuantityConvertedTo(existingIngredient.Quantity.Unit);
        var combinedQuantity = existingIngredient.Quantity + convertedIngredient.Quantity;

        group.Update(existingIngredient.WithQuantity(combinedQuantity));
        _logger.LogDebug("Updated ingredient {Ingredient} to {CombinedQuantity}",
            existingIngredient, combinedQuantity);
    }

    public Ingredient RemoveByQuantity(IngredientGroup group, Ingredient ingredient)
    {
        var existingIngredient = group.TryGetByProductId(ingredient.Product.Id);
        if (existingIngredient is null)
        {
            group.Add(ingredient);

            _logger.LogDebug("Added ingredient {Ingredient}", ingredient);
            return ingredient.Empty;
        }

        ThrowIfNotConvertible(ingredient, existingIngredient);

        var convertedIngredient = ingredient.WithQuantityConvertedTo(existingIngredient.Quantity.Unit);

        if (existingIngredient.Quantity <= convertedIngredient.Quantity)
        {
            group.Remove(existingIngredient);
            _logger.LogDebug("Removed ingredient {Ingredient}", existingIngredient);
            return convertedIngredient.WithQuantity(convertedIngredient.Quantity - existingIngredient.Quantity);
        }

        var combinedQuantity = existingIngredient.Quantity - convertedIngredient.Quantity;

        group.Update(existingIngredient.WithQuantity(combinedQuantity));
        _logger.LogDebug("Updated ingredient {Ingredient} to {CombinedQuantity}",
            existingIngredient, combinedQuantity);
        return convertedIngredient.Empty;
    }

    private void ThrowIfNotConvertible(Ingredient ingredient, Ingredient existingIngredient)
    {
        if (ingredient.Quantity.IsConvertibleToWithAdditionalInfo(existingIngredient.Quantity.Unit))
        {
            return;
        }

        _logger.LogError("Ingredient {Ingredient} is not compatible to {ExistingIngredient}",
            ingredient, existingIngredient);
        throw new IngredientQuantityUnitMismatch(ingredient, existingIngredient);
    }
}