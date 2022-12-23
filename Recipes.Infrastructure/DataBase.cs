using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Entities.RecipeAggregate;
using System.Xml.Serialization;
using Recipes.Domain.ValueObjects;

namespace Recipes.Infrastructure
{
    public class DataBase : IDataBase
    {
        public void InsertProduct(Product obj)
        {
            var products = GetAllProducts();

            products.Add(obj);

            var xmlSerializer = new XmlSerializer(products.GetType());

            using FileStream fs = new("Products.xml", FileMode.OpenOrCreate);
            xmlSerializer.Serialize(fs, products);
        }

        public List<Product> GetAllProducts()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Product>));
            var productsPath = "Products.xml";

            if (!File.Exists(productsPath))
            {
                return new List<Product>();
            }

            using var stream = new FileStream(productsPath, FileMode.Open);
            return (List<Product>)xmlSerializer.Deserialize(stream)!;
        }

        public void InsertRecipe(Recipe obj)
        {
            var recipes = GetAllRecipes();

            recipes.Add(obj);

            var xmlSerializer = new XmlSerializer(recipes.GetType());

            using FileStream fs = new("Recipes.xml", FileMode.OpenOrCreate);
            xmlSerializer.Serialize(fs, recipes);
        }

        public List<Recipe> GetAllRecipes()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Recipe>));
            var recipesPath = "Recipes.xml";

            if (!File.Exists(recipesPath))
            {
                return new List<Recipe>();
            }

            using var stream = new FileStream(recipesPath, FileMode.Open);
            return (List<Recipe>)xmlSerializer.Deserialize(stream)!;
        }
    }
}