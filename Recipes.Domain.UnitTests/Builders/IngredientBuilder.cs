using System.Collections.Generic;
using Recipes.Domain.Entities.IngredientAggregate;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Enums;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.UnitTests.Builders;

public class IngredientBuilder : BaseEntityBuilder<Ingredient, IngredientBuilder>
{
    private Product _product = new ProductBuilder().Build();
    private Quantity _quantity = new(1, QuantityUnit.Pieces);

    public IngredientBuilder WithProduct(Product product)
    {
        _product = product;
        return this;
    }

    public IngredientBuilder WithQuantity(Quantity quantity)
    {
        _quantity = quantity;
        return this;
    }

    protected override IEnumerable<object?> GetConstructorArguments()
    {
        yield return _product;
        yield return _quantity;
    }
}