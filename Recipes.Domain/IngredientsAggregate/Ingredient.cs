using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.IngredientsAggregate;

public class Ingredient : ValueObject<Ingredient>
{
    public Product Product { get; }
    public Quantity Quantity { get; }

    [EqualityIgnore] public Ingredient Empty => new(Product, Quantity.Empty);

    public Ingredient(Product product, Quantity quantity)
    {
        Product = Guard.Against.Null(product);
        Quantity = Guard.Against.Null(quantity);
    }

    public Ingredient WithQuantity(Quantity quantity) => new(Product, quantity);

    public Ingredient WithProduct(Product product) => new(product, Quantity);

    public override string ToString() => $"{Product} {Quantity}";
}