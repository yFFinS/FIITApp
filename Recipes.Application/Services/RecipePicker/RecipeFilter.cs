using Ardalis.GuardClauses;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Application.Services.RecipePicker;

public class RecipeFilter
{
    private readonly HashSet<Product> _disallowedProducts = new();
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

    public void DisallowProduct(Product product)
    {
        _disallowedProducts.Add(product);
    }

    public void AllowProduct(Product product)
    {
        _disallowedProducts.Remove(product);
    }

    public void AddOption(ProductFilterOption option)
    {
        _productOptions.Add(option.Product.Id, option);
    }

    public void RemoveOption(ProductFilterOption option)
    {
        _productOptions.Remove(option.Product.Id);
    }

    public bool IsAllowed(Product product)
    {
        return !_disallowedProducts.Contains(product);
    }

    public ProductFilterOption? GetOption(Product product)
    {
        return _productOptions.TryGetValue(product.Id, out var option) ? option : null;
    }
    
    public int OptionCount => _productOptions.Count;
}