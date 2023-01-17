using Recipes.Domain.Interfaces;
using System.Xml.Serialization;

namespace Recipes.Infrastructure;

public record DataBaseOptions(string ProductsPath, string RecipesPath, string dbAccessFileName) : IOptions;

public class DataBase : IDataBase
{
    private List<RecipeDbo> _recipes = null!;
    private List<ProductDbo> _products = null!;

    private bool _productsIsDirty = true;
    private bool _recipesIsDirty = true;

    private readonly DataBaseOptions _options;

    public DataBase(DataBaseOptions options)
    {
        _options = options;

        FTPServices.CheckForUpdates(_options.dbAccessFileName, "Products");
        FTPServices.CheckForUpdates(_options.dbAccessFileName, "Recipes");
    }

    private static void AddOrUpdate<T>(T obj, IList<T> dest) where T : IEquatable<T>
    {
        for (var i = 0; i < dest.Count; i++)
        {
            if (dest[i].Equals(obj))
            {
                dest[i] = obj;
                return;
            }
        }

        dest.Add(obj);
    }

    public void InsertProduct(ProductDbo product)
    {
        var products = GetAllProducts();
        AddOrUpdate(product, products);
        Serialize(products, _options.ProductsPath);

        _productsIsDirty = true;
    }

    public void InsertRecipe(RecipeDbo obj)
    {
        var recipes = GetAllRecipes();
        AddOrUpdate(obj, recipes);
        Serialize(recipes, _options.RecipesPath);

        _recipesIsDirty = true;
    }


    public List<ProductDbo> GetAllProducts()
    {
        if (!_productsIsDirty)
        {
            return _products;
        }

        _products = Deserialize<List<ProductDbo>>(_options.ProductsPath) ?? new List<ProductDbo>();
        _productsIsDirty = false;
        return _products;
    }

    public List<RecipeDbo> GetAllRecipes()
    {
        if (!_recipesIsDirty)
        {
            return _recipes;
        }

        _recipes = Deserialize<List<RecipeDbo>>(_options.RecipesPath) ?? new List<RecipeDbo>();
        _recipesIsDirty = false;
        return _recipes;
    }


    private static void Serialize<T>(T obj, string path) where T : notnull
    {
        var xmlSerializer = new XmlSerializer(obj.GetType());
        using FileStream fs = new(path, FileMode.OpenOrCreate);
        xmlSerializer.Serialize(fs, obj);
    }

    private static T? Deserialize<T>(string path)
    {
        var xmlSerializer = new XmlSerializer(typeof(T));
        if (!File.Exists(path))
        {
            return default;
        }

        using var stream = new FileStream(path, FileMode.Open);
        return (T)xmlSerializer.Deserialize(stream)!;
    }
}