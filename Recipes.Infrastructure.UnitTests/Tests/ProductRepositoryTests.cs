using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;
using Recipes.Tests.Shared.BuilderEntries;

namespace Recipes.Infrastructure.UnitTests;

[TestFixture]
public class ProductRepositoryTests
{
    [Test]
    public void Test_GetProductsProducts()
    {
        var fakeDatabase = new FakeDatabase();
        var fakeQuantityUnitRepository = new FakeQuantityUnitRepository();
        var repository = new ProductRepository(NullLogger<ProductRepository>.Instance,
            fakeDatabase, fakeQuantityUnitRepository);

        var products = repository.GetAllProductsAsync().Result;
        var productNames = products.Select(p => p.Name).ToList();
        var expectedProductNames = fakeDatabase.TestProducts.Select(p => p.Name).ToList();

        Assert.That(productNames, Is.EquivalentTo(expectedProductNames));
    }

    [Test]
    public void Test_AddProducts()
    {
        var fakeDatabase = new FakeDatabase();
        var fakeQuantityUnitRepository = new FakeQuantityUnitRepository();
        var repository = new ProductRepository(NullLogger<ProductRepository>.Instance,
            fakeDatabase, fakeQuantityUnitRepository);

        var products = new[] { A.Product.Build(), A.Product.Build() };
        repository.AddProductsAsync(products).Wait();
        var repositoryProducts = repository.GetAllProductsAsync().Result;

        var productIds = repositoryProducts.Select(p => p.Id.ToString()).ToList();
        var expectedProductNames = fakeDatabase.TestProducts
            .Select(p => p.Id.ToString())
            .Concat(products.Select(p => p.Id.ToString()))
            .ToList();

        Assert.That(productIds, Is.EquivalentTo(expectedProductNames));
    }

    [Test]
    public void Test_GetProductById()
    {
        var fakeDatabase = new FakeDatabase();
        var fakeQuantityUnitRepository = new FakeQuantityUnitRepository();
        var repository = new ProductRepository(NullLogger<ProductRepository>.Instance,
            fakeDatabase, fakeQuantityUnitRepository);

        var testProductId = fakeDatabase.TestProducts[0].Id;
        var product = repository.GetProductByIdAsync(new EntityId(testProductId)).Result;

        Assert.That(product, Is.Not.Null);
        Assert.That(product!.Id.ToString(), Is.EqualTo(fakeDatabase.TestProducts[0].Id));
    }
}

public class FakeDatabase : IDataBase
{
    public IReadOnlyList<ProductDbo> TestProducts { get; } = new List<ProductDbo>()
    {
        new()
        {
            Id = EntityId.NewId().ToString(),
            Name = "Продукт 1",
        },
        new()
        {
            Id = EntityId.NewId().ToString(),
            Name = "Продукт 2",
        },
        new()
        {
            Id = EntityId.NewId().ToString(),
            Name = "Продукт 3",
        }
    };

    private readonly List<ProductDbo> _products;

    public FakeDatabase()
    {
        _products = new List<ProductDbo>(TestProducts);
    }

    public void InsertProduct(ProductDbo product)
    {
        _products.Add(product);
    }

    public List<ProductDbo> GetAllProducts()
    {
        return _products;
    }

    public void InsertRecipe(RecipeDbo obj) => throw new NotImplementedException();

    public List<RecipeDbo> GetAllRecipes() => throw new NotImplementedException();
}

public class FakeQuantityUnitRepository : IQuantityUnitRepository
{
    private readonly Dictionary<int, QuantityUnit> _quantityUnits = new()
    {
        { 1, new QuantityUnit("граммы", "г", gramsConversionFactor: 1) },
        { 2, new QuantityUnit("штуки", "шт") },
        { 3, new QuantityUnit("литры", "л", millilitersConversionFactor: 1000) },
    };

    public IReadOnlyList<QuantityUnit> GetAllUnits() => _quantityUnits.Values.ToList();

    public QuantityUnit? GetUnitById(int id) => _quantityUnits.TryGetValue(id, out var unit) ? unit : null;

    public int GetUnitId(QuantityUnit unit) => _quantityUnits.FirstOrDefault(x => x.Value == unit).Key;
}