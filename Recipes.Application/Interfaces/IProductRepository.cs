using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Application.Interfaces;

public interface IProductRepository
{
    List<Product> GetAllProducts();
    Product? GetProductById(EntityId productId);
    Product? GetProductByName(string productName);
    List<Product> GetProductsByPrefix(string productNamePrefix);
    void AddProducts(IEnumerable<Product> products);
    void RemoveProductsById(IEnumerable<EntityId> products);
}