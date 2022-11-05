using Ardalis.GuardClauses;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Application.Services.RecipePicker;

public class RecipeFilter
{
    private readonly HashSet<EntityId> _disallowedProducts = new();
    private readonly Dictionary<EntityId, ProductFilterOption> _productOptions = new();

    private int _maxResults = 10;
    private TimeSpan? _maxCookDuration;

    public int MaxRecipes
    {
        get => _maxResults;
        set => _maxResults = Guard.Against.NegativeOrZero(value);
    }

    public TimeSpan? MaxCookDuration
    {
        get => _maxCookDuration;
        set => _maxCookDuration = value.HasValue ? Guard.Against.NegativeOrZero(value.Value) : null;
    }

    public void DisallowProduct(EntityId productId)
    {
        _disallowedProducts.Add(productId);
    }

    public void AllowProduct(EntityId productId)
    {
        _disallowedProducts.Remove(productId);
    }

    public void AddOption(ProductFilterOption option)
    {
        _productOptions.Add(option.Product.Id, option);
    }

    public void RemoveOption(ProductFilterOption option)
    {
        _productOptions.Remove(option.Product.Id);
    }

    public bool IsAllowed(EntityId productId)
    {
        return !_disallowedProducts.Contains(productId);
    }

    public ProductFilterOption? GetOption(EntityId productId)
    {
        return _productOptions.TryGetValue(productId, out var option) ? option : null;
    }

    public int OptionCount => _productOptions.Count;
}