using Recipes.Domain.Base;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Recipes.Infrastructure
{
    public static class DataBase
    {
        private static string GetPath()
        {
            var slash = Path.GetFullPath("/")[2..];
            var rootPath = Environment.CurrentDirectory.Split(slash);
            while (rootPath[rootPath.Length - 1] != "FIITApp")
            {
                rootPath = rootPath[..(rootPath.Length - 2)];
            }

            return string.Join(slash, rootPath) + slash;
        }

        public static void InsertProduct(Product obj)
        {
            var products = GetAllProducts();
            products ??= new List<Product>();

            products.Add(obj);

            var xmlSerializer = new XmlSerializer(products.GetType());

            string path = GetPath();

            using FileStream fs = new(path + "Products.xml", FileMode.OpenOrCreate);
            xmlSerializer.Serialize(fs, products);
        }

        public static List<Product> GetAllProducts()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Product>));

            string path = GetPath();
            var productsPath = path + "Products.xml";

            if (!File.Exists(productsPath))
            {
                return new List<Product>();
            }

            using var stream = new FileStream(productsPath, FileMode.Open);
            return (List<Product>)xmlSerializer.Deserialize(stream)!;
        }

        public static void InsertRecipe(Recipe obj)
        {
            var recipes = GetAllRecipes();
            recipes ??= new List<Recipe>();

            recipes.Add(obj);

            var xmlSerializer = new XmlSerializer(recipes.GetType());

            string path = GetPath();

            using FileStream fs = new(path + "Recipes.xml", FileMode.OpenOrCreate);
            xmlSerializer.Serialize(fs, recipes);
        }

        public static List<Recipe> GetAllRecipes()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Recipe>));

            string path = GetPath();
            var recipesPath = path + "Recipes.xml";

            if (!File.Exists(recipesPath))
            {
                return new List<Recipe>();
            }

            using var stream = new FileStream(recipesPath, FileMode.Open);
            return (List<Recipe>)xmlSerializer.Deserialize(stream)!;
        }
    }
}