using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Application.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(EntityId productId);
    Task<Product?> GetProductByNameAsync(string productName);
    Task<List<Product>> GetProductsByPrefixAsync(string productNamePrefix);
    Task AddProductsAsync(IEnumerable<Product> products);
    Task RemoveProductsByIdAsync(IEnumerable<EntityId> products);
}