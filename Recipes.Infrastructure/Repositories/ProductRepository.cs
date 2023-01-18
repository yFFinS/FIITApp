using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;

namespace Recipes.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ILogger<ProductRepository> _logger;
    private readonly IDataBase _dataBase;
    private readonly IQuantityUnitRepository _quantityUnitRepository;

    private Dictionary<EntityId, Product>? _products;

    public ProductRepository(ILogger<ProductRepository> logger, IDataBase dataBase,
        IQuantityUnitRepository quantityUnitRepository)
    {
        _logger = logger;
        _dataBase = dataBase;
        _quantityUnitRepository = quantityUnitRepository;
    }

    private Dictionary<EntityId, Product> GetProductMapping()
    {
        if (_products is null)
        {
            _logger.LogInformation("Getting all products");
            var productDbos = _dataBase.GetAllProducts();
            var products = productDbos.Select(DboToProduct).ToList();
            _products = products.ToDictionary(p => p.Id);
        }

        return _products;
    }

    public List<Product> GetAllProducts()
    {
        var productMapping = GetProductMapping();
        return productMapping.Values.ToList();
    }

    public Product? GetProductById(EntityId productId)
    {
        _logger.LogInformation("Getting product by id {ProductId}", productId);
        var products = GetProductMapping();
        return products.GetValueOrDefault(productId);
    }

    public Product? GetProductByName(string productName)
    {
        _logger.LogInformation("Getting product by name {ProductName}", productName);
        var products = GetAllProducts();
        return products.FirstOrDefault(p => p.Name == productName);
    }

    public List<Product> GetProductsBySubstring(string substring)
    {
        substring = substring.ToLower();
        _logger.LogInformation("Getting products by substring {Substring}", substring);
        var products = GetAllProducts();
        return products.Where(p =>
                p.Name.ToLower().Contains(substring))
            .Select(p => (Product: p, Score: p.Name.ToLower().Split().Contains(substring) ? 1 : 0))
            .OrderByDescending(r => r.Score)
            .ThenBy(r => r.Product.Name)
            .Select(r => r.Product)
            .ToList();
    }

    public void AddProducts(IEnumerable<Product> products)
    {
        foreach (var product in products)
        {
            _logger.LogInformation("Adding product {ProductName} to database", product.Name);
            var productDbo = ProductToDbo(product);
            _dataBase.InsertProduct(productDbo);
        }
    }

    public void RemoveProductsById(IEnumerable<EntityId> products)
    {
        foreach (var productId in products)
        {
            _logger.LogInformation("Removing product {ProductId} from database", productId);
            _dataBase.DeleteProduct(productId);
        }
    }

    private ProductDbo ProductToDbo(Product product)
    {
        var units = product.ValidQuantityUnits.Select(u => _quantityUnitRepository.GetUnitId(u)).ToArray();
        return new ProductDbo
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Description = product.Description,
            PieceWeight = product.PieceWeight,
            ImageUrl = product.ImageUrl?.ToString(),
            QuantityUnitIds = units
        };
    }

    private Product DboToProduct(ProductDbo productDbo)
    {
        var quantityUnits = productDbo.QuantityUnitIds
            .Select(id => (id, _quantityUnitRepository.GetUnitById(id)))
            .ToArray();

        var existingQuantityUnitIds = new List<QuantityUnit>();
        foreach (var (id, quantityUnit) in quantityUnits)
        {
            if (quantityUnit is null)
            {
                _logger.LogWarning("Quantity unit with id {Id} does not exist", id);
                continue;
            }

            existingQuantityUnitIds.Add(quantityUnit);
        }

        var uri = productDbo.ImageUrl is null ? null : new Uri(productDbo.ImageUrl);
        return new Product(new EntityId(productDbo.Id), productDbo.Name, productDbo.Description,
            existingQuantityUnitIds)
        {
            ImageUrl = uri,
            PieceWeight = productDbo.PieceWeight
        };
    }
}