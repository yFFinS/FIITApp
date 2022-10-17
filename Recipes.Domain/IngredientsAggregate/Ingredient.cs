using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Enums;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.IngredientsAggregate;

public class Ingredient : ValueObject
{
    public readonly Product Product;
    public readonly Quantity Quantity;

    public EntityId ProductId => Product.Id;
    public Ingredient Empty => new(Product, Quantity.Empty);

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

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Product;
        yield return Quantity;
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