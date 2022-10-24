using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Enums;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.IngredientsAggregate;

public class Ingredient : ValueObject<Ingredient>
{
    public Product Product { get; }
    public Quantity Quantity { get; }


    [EqualityIgnore] public EntityId ProductId => Product.Id;
    [EqualityIgnore] public Ingredient Empty => new(Product, Quantity.Empty);

    public Ingredient(Product product, Quantity quantity)
    {
        Product = Guard.Against.Null(product);
        Quantity = Guard.Against.Null(quantity);
    }

    public Ingredient WithQuantity(Quantity quantity)
    {
        return new Ingredient(Product, quantity);
    }

    public Ingredient WithProduct(Product product)
    {
        return new Ingredient(product, Quantity);
    }

    public Ingredient WithQuantityConvertedTo(QuantityUnit unit)
    {
        // TODO: Implement this
        return new Ingredient(Product, Quantity.ImplicitlyConvertTo(unit));
    }

    public override string ToString()
    {
        return $"{Product} {Quantity}";
    }
}