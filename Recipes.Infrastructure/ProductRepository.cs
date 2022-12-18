using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Infrastructure;

public class ProductRepository : IProductRepository
{
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(ILogger<ProductRepository> logger)
    {
        _logger = logger;
    }

    public Task<List<Product>> GetAllProductsAsync()
    {
        _logger.LogInformation("Getting all products");
        var products = DataBase.GetAllProducts();
        return Task.FromResult(products);
    }

    public async Task<Product?> GetProductByIdAsync(EntityId productId)
    {
        _logger.LogInformation("Getting product by id {ProductId}", productId);
        var products = await GetAllProductsAsync();
        return products.FirstOrDefault(p => p.Id == productId);
    }

    public async Task<Product?> GetProductByNameAsync(string productName)
    {
        _logger.LogInformation("Getting product by name {ProductName}", productName);
        var products = await GetAllProductsAsync();
        return products.FirstOrDefault(p => p.Name == productName);
    }

    public async Task<List<Product>> GetProductsByPrefixAsync(string productNamePrefix)
    {
        _logger.LogInformation("Getting products by prefix {ProductNamePrefix}", productNamePrefix);
        var products = await GetAllProductsAsync();
        return products.Where(p => p.Name.ToLower().StartsWith(productNamePrefix)).ToList();
    }

    public Task AddProductsAsync(IEnumerable<Product> products)
    {
        foreach (var product in products)
        {
            _logger.LogInformation("Adding product {@Product} to database", product);
            DataBase.InsertProduct(product);
        }

        return Task.CompletedTask;
    }

    public Task RemoveProductsByIdAsync(IEnumerable<EntityId> products)
    {
        throw new NotImplementedException();
    }
}