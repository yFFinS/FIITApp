using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Enums;
using Recipes.Domain.ValueObjects;

namespace Recipes.Tests.Shared.Builders;

public class ProductBuilder : BaseEntityBuilder<Product, ProductBuilder>
{
    private string _name = "Product Name";
    private PriceForQuantity _priceForQuantity = new(new Price(10m), new Quantity(1, QuantityUnit.Cups));

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductBuilder WithPriceForQuantity(PriceForQuantity priceForQuantity)
    {
        _priceForQuantity = priceForQuantity;
        return this;
    }

    public override Product Build()
    {
        return new Product(Id, _name, _priceForQuantity);
    }
}