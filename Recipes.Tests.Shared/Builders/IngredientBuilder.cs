using Recipes.Domain.Enums;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.ValueObjects;
using Recipes.Tests.Shared.BuilderEntries;

namespace Recipes.Tests.Shared.Builders;

public class IngredientBuilder : AbstractBuilder<Ingredient>
{
    private EntityId _productId = An.EntityId;
    private Quantity _quantity = new(1, QuantityUnit.Pieces);

    public IngredientBuilder WithProductId(EntityId productId)
    {
        _productId = productId;
        return this;
    }

    public IngredientBuilder WithQuantity(Quantity quantity)
    {
        _quantity = quantity;
        return this;
    }

    public override Ingredient Build()
    {
        return new Ingredient(_productId, _quantity);
    }
}