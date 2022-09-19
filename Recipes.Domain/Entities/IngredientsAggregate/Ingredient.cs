using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.IngredientAggregate;

public class Ingredient : BaseEntity
{
    private Product _product = null!;
    private Quantity _quantity = null!;

    public Product Product
    {
        get => _product;
        private set => _product = Guard.Against.Null(value);
    }

    public Quantity Quantity
    {
        get => _quantity;
        private set => _quantity = Guard.Against.Null(value);
    }

    public Ingredient(EntityId id, Product product, Quantity quantity) : base(id)
    {
        Product = product;
        Quantity = quantity;
    }

    public void UpdateQuantity(Quantity quantity)
    {
        Quantity = quantity;
    }

    public void UpdateProduct(Product product)
    {
        Product = product;
    }
}