// using NUnit.Framework;
// using Recipes.Application.Services.Preferences;
// using Recipes.Application.Services.RecipePicker;
// using Recipes.Domain.Entities.ProductAggregate;
// using Recipes.Domain.IngredientsAggregate;
// using Recipes.Domain.ValueObjects;
// using Recipes.Tests.Shared.BuilderEntries;
// using System.Collections.Generic;
//
// namespace Recipes.Application.UnitTests.Tests;
//
// public class RecipePickerTests
// {
//     private static readonly Quantity Quantity = A.Quantity.WithValue(1);
//
//     private static readonly Quantity LesserQuantity = A.Quantity.WithValue(0.5);
//
//     [Test]
//     public void Test_DislikedProductsWithOptions()
//     {
//         var dislikedIngredients = new List<Ingredient>
//         {
//             An.Ingredient.WithQuantity(Quantity),
//             An.Ingredient.WithQuantity(Quantity),
//             An.Ingredient.WithQuantity(Quantity),
//         };
//
//         var preferences = new Preferences();
//         var recipe = A.Recipe.Build();
//         var filter = new RecipeFilter();
//
//         foreach (var dislikedIngredient in dislikedIngredients)
//         {
//             preferences.DislikeProduct(dislikedIngredient.Product.Id);
//             recipe.AddIngredient(dislikedIngredient);
//
//             var product = new Product(dislikedIngredient.Product.Id, "a");
//
//             var option = new ProductFilterOption(product, Quantity);
//             filter.AddOption(option);
//         }
//
//         var actualScore = RecipePicker.ScoreRecipe(recipe, filter, preferences);
//
//         var trueScore =
//             RecipePicker.dislikedProductPenalty * 3 +
//             RecipePicker.hasProductScore * 3 +
//             RecipePicker.allOptionsSatisfiedScore;
//
//         Assert.AreEqual(trueScore, actualScore);
//     }
//
//     [Test]
//     public void Test_DislikedProductsWithoutOptions()
//     {
//         var dislikedProducts = new List<Ingredient>
//         {
//             new Ingredient(EntityId.NewId(), Quantity),
//             new Ingredient(EntityId.NewId(), Quantity),
//             new Ingredient(EntityId.NewId(), Quantity)
//         };
//
//         var preferences = new Preferences();
//
//         var recipe = A.Recipe.Build();
//
//         var filter = new RecipeFilter();
//
//         foreach (var dislikedProduct in dislikedProducts)
//         {
//             preferences.DislikeProduct(dislikedProduct.ProductId);
//             recipe.AddIngredient(dislikedProduct);
//         }
//
//         var actualScore = RecipePicker.ScoreRecipe(recipe, filter, preferences);
//
//         var trueScore =
//             RecipePicker.allOptionsSatisfiedScore;
//
//         Assert.AreEqual(trueScore, actualScore);
//     }
//
//     [Test]
//     public void Test_LikedProductsWithOptions()
//     {
//         var likedProducts = new List<Ingredient>
//         {
//             new Ingredient(EntityId.NewId(), Quantity),
//             new Ingredient(EntityId.NewId(), Quantity),
//             new Ingredient(EntityId.NewId(), Quantity)
//         };
//
//         var preferences = new Preferences();
//
//         var recipe = A.Recipe.Build();
//
//         var filter = new RecipeFilter();
//
//         foreach (var likedProduct in likedProducts)
//         {
//             preferences.LikeProduct(likedProduct.ProductId);
//             recipe.AddIngredient(likedProduct);
//
//             var product = new Product(likedProduct.ProductId, "a");
//
//             var option = new ProductFilterOption(product, Quantity);
//             filter.AddOption(option);
//         }
//
//         var actualScore = RecipePicker.ScoreRecipe(recipe, filter, preferences);
//
//         var trueScore =
//             RecipePicker.likedProductScore * 3 +
//             RecipePicker.hasProductScore * 3 +
//             RecipePicker.allOptionsSatisfiedScore;
//
//         Assert.AreEqual(trueScore, actualScore);
//     }
//
//     [Test]
//     public void Test_LikedProductsWithoutOptions()
//     {
//         var likedProducts = new List<Ingredient>
//         {
//             new Ingredient(EntityId.NewId(), Quantity),
//             new Ingredient(EntityId.NewId(), Quantity),
//             new Ingredient(EntityId.NewId(), Quantity)
//         };
//
//         var preferences = new Preferences();
//
//         var recipe = A.Recipe.Build();
//
//         var filter = new RecipeFilter();
//
//         foreach (var likedProduct in likedProducts)
//         {
//             preferences.LikeProduct(likedProduct.ProductId);
//             recipe.AddIngredient(likedProduct);
//         }
//
//         var actualScore = RecipePicker.ScoreRecipe(recipe, filter, preferences);
//
//         var trueScore =
//             RecipePicker.allOptionsSatisfiedScore;
//
//         Assert.AreEqual(trueScore, actualScore);
//     }
//
//     [Test]
//     public void Test_EnoughProducts()
//     {
//         var products = new List<Ingredient>
//         {
//             new Ingredient(EntityId.NewId(), Quantity),
//             new Ingredient(EntityId.NewId(), Quantity),
//             new Ingredient(EntityId.NewId(), Quantity)
//         };
//
//         var preferences = new Preferences();
//
//         var recipe = A.Recipe.Build();
//
//         var filter = new RecipeFilter();
//
//         foreach (var product in products)
//         {
//             recipe.AddIngredient(product);
//
//             var actualProduct = new Product(product.ProductId, "a");
//
//             var option = new ProductFilterOption(actualProduct, Quantity);
//             filter.AddOption(option);
//         }
//
//         var actualScore = RecipePicker.ScoreRecipe(recipe, filter, preferences);
//
//         var trueScore =
//             RecipePicker.hasProductScore * 3 +
//             RecipePicker.allOptionsSatisfiedScore;
//
//         Assert.AreEqual(trueScore, actualScore);
//     }
//
//     [Test]
//     public void Test_NotEnoughProducts()
//     {
//         var products = new List<Ingredient>
//         {
//             new Ingredient(EntityId.NewId(), Quantity),
//             new Ingredient(EntityId.NewId(), Quantity),
//             new Ingredient(EntityId.NewId(), Quantity)
//         };
//
//         var preferences = new Preferences();
//
//         var recipe = A.Recipe.Build();
//
//         var filter = new RecipeFilter();
//
//         foreach (var product in products)
//         {
//             recipe.AddIngredient(product);
//
//             var actualProduct = new Product(product.ProductId, "a");
//
//             var option = new ProductFilterOption(actualProduct, LesserQuantity);
//             filter.AddOption(option);
//         }
//
//         var actualScore = RecipePicker.ScoreRecipe(recipe, filter, preferences);
//
//         var trueScore =
//             RecipePicker.notEnoughProductsPenalty * 3;
//
//         Assert.AreEqual(trueScore, actualScore);
//     }
// }