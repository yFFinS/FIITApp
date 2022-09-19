using System.Collections.Generic;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Enums;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.UnitTests.Builders;

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

    protected override IEnumerable<object?> GetConstructorArguments()
    {
        yield return _name;
        yield return _priceForQuantity;
    }
}