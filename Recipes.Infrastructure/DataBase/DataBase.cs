using Recipes.Domain.ValueObjects;
using System.Xml.Serialization;

namespace Recipes.Infrastructure.DataBase;

public class DataBase : IDataBase
{
    private List<RecipeDbo> _recipes = null!;
    private List<ProductDbo> _products = null!;
    private List<RecipeDbo> _userRecipes = null!;

    private bool _productsIsDirty = true;
    private bool _recipesIsDirty = true;
    private bool _userRecipesIsDirty = true;

    private readonly DatabasePathsProvider _pathsProvider;

    public DataBase(DatabasePathsProvider pathsProvider)
    {
        _pathsProvider = pathsProvider;
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
        Serialize(products, _pathsProvider.GetDatabasePath(DatabaseName.Products));

        _productsIsDirty = true;
    }

    public void InsertRecipe(RecipeDbo recipe, bool useUserDatabase)
    {
        var recipes = useUserDatabase ? GetUserRecipes() : GetGlobalRecipes();
        var databaseName = useUserDatabase ? DatabaseName.CustomRecipes : DatabaseName.Recipes;

        AddOrUpdate(recipe, recipes);
        Serialize(recipes, _pathsProvider.GetDatabasePath(databaseName));

        if (useUserDatabase)
        {
            _userRecipesIsDirty = true;
        }
        else
        {
            _recipesIsDirty = true;
        }
    }

    private List<RecipeDbo> GetUserRecipes()
    {
        if (_userRecipesIsDirty)
        {
            _userRecipes = Deserialize<List<RecipeDbo>>(_pathsProvider.GetDatabasePath(DatabaseName.CustomRecipes)) ??
                           new List<RecipeDbo>();
            _userRecipesIsDirty = false;
        }

        return _userRecipes;
    }

    private List<RecipeDbo> GetGlobalRecipes()
    {
        if (_recipesIsDirty)
        {
            _recipes = Deserialize<List<RecipeDbo>>(_pathsProvider.GetDatabasePath(DatabaseName.Recipes)) ??
                       new List<RecipeDbo>();
            _recipesIsDirty = false;
        }

        return _recipes;
    }

    public List<ProductDbo> GetAllProducts()
    {
        if (!_productsIsDirty)
        {
            return _products;
        }

        _products = Deserialize<List<ProductDbo>>(_pathsProvider.GetDatabasePath(DatabaseName.Products)) ??
                    new List<ProductDbo>();
        _productsIsDirty = false;

        return _products.ToList();
    }

    public List<RecipeDbo> GetAllRecipes(bool onlyGlobal)
    {
        var globalRecipes = GetGlobalRecipes();
        return onlyGlobal ? globalRecipes : globalRecipes.Concat(GetUserRecipes()).ToList();
    }

    public void DeleteProduct(EntityId product)
    {
        var stringId = product.ToString();
        var products = GetAllProducts();
        products.RemoveAll(x => x.Id == stringId);

        Serialize(products, _pathsProvider.GetDatabasePath(DatabaseName.Products));
        _productsIsDirty = true;
    }

    public void DeleteRecipe(EntityId recipe)
    {
        var stringId = recipe.ToString();
        var recipes = GetGlobalRecipes();
        recipes.RemoveAll(x => x.Id == stringId);

        Serialize(recipes, _pathsProvider.GetDatabasePath(DatabaseName.Recipes));
        _recipesIsDirty = true;
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