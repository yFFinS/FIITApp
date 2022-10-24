using Ardalis.GuardClauses;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Application.Services.RecipePicker;

public class ProductFilterOption
{
    public readonly Product Product;
    public readonly Quantity? Quantity;

    public ProductFilterOption(Product product, Quantity? quantity = null)
    {
        Product = Guard.Against.Null(product);
        Quantity = quantity;
    }
}