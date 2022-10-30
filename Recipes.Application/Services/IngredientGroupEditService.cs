using Microsoft.Extensions.Logging;
using Recipes.Application.Exceptions;
using Recipes.Application.Interfaces;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.Interfaces;

namespace Recipes.Application.Services;

public class IngredientGroupEditService : IIngredientGroupEditService
{
    private readonly ILogger<IngredientGroupEditService> _logger;
    private readonly IQuantityConverter _quantityConverter;

    public IngredientGroupEditService(ILogger<IngredientGroupEditService> logger, IQuantityConverter quantityConverter)
    {
        _logger = logger;
        _quantityConverter = quantityConverter;
    }

    public void AddByQuantity(IngredientGroup group, Ingredient ingredient)
    {
        var existingIngredient = group.TryGetByProductId(ingredient.ProductId);
        if (existingIngredient is null)
        {
            group.Add(ingredient);

            _logger.LogDebug("Added ingredient {Ingredient}", ingredient);
            return;
        }

        var convertedIngredient = ConvertedIngredientQuantity(ingredient, existingIngredient);
        var combinedQuantity = existingIngredient.Quantity + convertedIngredient.Quantity;

        group.Update(existingIngredient.WithQuantity(combinedQuantity));
        _logger.LogDebug("Updated ingredient {Ingredient} to {CombinedQuantity}",
            existingIngredient, combinedQuantity);
    }

    private Ingredient ConvertedIngredientQuantity(Ingredient ingredient, Ingredient existingIngredient)
    {
        ThrowIfNotConvertible(ingredient, existingIngredient);
        var convertedQuantity =
            _quantityConverter.Convert(ingredient.Quantity, existingIngredient.Quantity.Unit, ingredient.ProductId);
        var convertedIngredient = ingredient.WithQuantity(convertedQuantity);
        return convertedIngredient;
    }

    public Ingredient RemoveByQuantity(IngredientGroup group, Ingredient ingredient)
    {
        var existingIngredient = group.TryGetByProductId(ingredient.ProductId);
        if (existingIngredient is null)
        {
            group.Add(ingredient);

            _logger.LogDebug("Added ingredient {Ingredient}", ingredient);
            return ingredient.Empty;
        }

        ThrowIfNotConvertible(ingredient, existingIngredient);

        var convertedIngredient = ConvertedIngredientQuantity(ingredient, existingIngredient);

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

    private void ThrowIfNotConvertible(Ingredient fromIngredient, Ingredient toIngredient)
    {
        if (_quantityConverter.CanConvert(fromIngredient.Quantity.Unit, toIngredient.Quantity.Unit,
                fromIngredient.ProductId))
        {
            return;
        }

        _logger.LogError("Ingredient {FromIngredient} is not compatible to {ToIngredient}",
            fromIngredient, toIngredient);
        throw new IngredientQuantityUnitMismatch(fromIngredient, toIngredient);
    }
}