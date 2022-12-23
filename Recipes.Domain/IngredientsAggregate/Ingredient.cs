using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.IngredientsAggregate;

[Serializable]
public class Ingredient : ValueObject<Ingredient>
{
    private readonly EntityId _productId;
    private readonly Quantity _quantity;

    public EntityId ProductId
    {
        get => _productId;
        set => throw new NotSupportedException();
    }

    public Quantity Quantity
    {
        get => _quantity;
        set => throw new NotSupportedException();
    }
    
    private Ingredient() {}

    [EqualityIgnore] public Ingredient Empty => new(ProductId, Quantity.Empty);

    public Ingredient(EntityId productId, Quantity quantity)
    {
        _productId = Guard.Against.Null(productId);
        _quantity = Guard.Against.Null(quantity);
    }

    public Ingredient WithQuantity(Quantity quantity)
    {
        return new Ingredient(ProductId, quantity);
    }

    public Ingredient WithProduct(EntityId productId)
    {
        return new Ingredient(productId, Quantity);
    }

    public override string ToString()
    {
        return $"{ProductId} {Quantity}";
    }
}