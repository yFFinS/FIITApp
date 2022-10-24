using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Enums;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Tests.Shared.Builders;

public class IngredientBuilder : AbstractBuilder<Ingredient>
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

    public override Ingredient Build()
    {
        return new Ingredient(_product, _quantity);
    }
}