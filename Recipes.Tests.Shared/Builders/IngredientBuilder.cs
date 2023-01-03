using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.ValueObjects;
using Recipes.Tests.Shared.BuilderEntries;

namespace Recipes.Tests.Shared.Builders;

public class IngredientBuilder : AbstractBuilder<Ingredient>
{
    private Product _product = A.Product;
    private Quantity _quantity = A.Quantity;

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