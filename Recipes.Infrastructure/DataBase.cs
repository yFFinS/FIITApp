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
        public static void InsertProduct(Product obj)
        {
            var products = GetAllProducts();
            products ??= new List<Product>();

            products.Add(obj);

            var xmlSerializer = new XmlSerializer(products.GetType());

            using FileStream fs = new("Products.xml", FileMode.OpenOrCreate);
            xmlSerializer.Serialize(fs, products);
        }

        private static List<Product>? GetAllProducts()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Product>));

            using FileStream fs = new("Products.xml", FileMode.OpenOrCreate);
            return xmlSerializer.Deserialize(fs) as List<Product>;
        }

        public static void InsertRecipe(Recipe obj)
        {
            var recipes = GetAllRecipes();
            recipes ??= new List<Recipe>();

            recipes.Add(obj);

            var xmlSerializer = new XmlSerializer(recipes.GetType());

            using FileStream fs = new("Recipes.xml", FileMode.OpenOrCreate);
            xmlSerializer.Serialize(fs, recipes);
        }

        private static List<Recipe>? GetAllRecipes()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Recipe>));

            using FileStream fs = new("Recipes.xml", FileMode.OpenOrCreate);
            return xmlSerializer.Deserialize(fs) as List<Recipe>;
        }
    }
}
