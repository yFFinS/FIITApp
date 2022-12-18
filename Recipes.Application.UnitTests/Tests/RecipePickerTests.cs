using NUnit.Framework;
using Recipes.Application.Services.Preferences;
using Recipes.Application.Services.RecipePicker;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.ValueObjects;
using Recipes.Tests.Shared.BuilderEntries;
using System.Collections.Generic;

namespace Recipes.Application.UnitTests.Tests;

public class RecipePickerTests
{
    private static readonly Quantity quantity = new(1, Domain.Enums.QuantityUnit.Pieces);

    private static readonly Quantity lessQuantity = new(0.5, Domain.Enums.QuantityUnit.Pieces);

    [Test]
    public void Test_DislikedProductsWithOptions()
    {
        var dislikedProducts = new List<Ingredient>
        {
            new Ingredient(EntityId.NewId(), quantity),
            new Ingredient(EntityId.NewId(), quantity),
            new Ingredient(EntityId.NewId(), quantity)
        };

        var preferences = new Preferences();

        var recipe = A.Recipe.Build();

        var filter = new RecipeFilter();

        foreach (var dislikedProduct in dislikedProducts)
        {
            preferences.DislikeProduct(dislikedProduct.ProductId);
            recipe.AddIngredient(dislikedProduct);

            var product = new Product(dislikedProduct.ProductId, "a");

            var option = new ProductFilterOption(product, quantity);
            filter.AddOption(option);
        }

        var score = RecipePicker.ScoreRecipe(recipe, filter, preferences);

        Assert.AreEqual(-20, score);
    }

    [Test]
    public void Test_DislikedProductsWithoutOptions()
    {
        var dislikedProducts = new List<Ingredient>
        {
            new Ingredient(EntityId.NewId(), quantity),
            new Ingredient(EntityId.NewId(), quantity),
            new Ingredient(EntityId.NewId(), quantity)
        };

        var preferences = new Preferences();

        var recipe = A.Recipe.Build();

        var filter = new RecipeFilter();

        foreach (var dislikedProduct in dislikedProducts)
        {
            preferences.DislikeProduct(dislikedProduct.ProductId);
            recipe.AddIngredient(dislikedProduct);
        }

        var score = RecipePicker.ScoreRecipe(recipe, filter, preferences);

        Assert.AreEqual(100, score);
    }

    [Test]
    public void Test_LikedProductsWithOptions()
    {
        var likedProducts = new List<Ingredient>
        {
            new Ingredient(EntityId.NewId(), quantity),
            new Ingredient(EntityId.NewId(), quantity),
            new Ingredient(EntityId.NewId(), quantity)
        };

        var preferences = new Preferences();

        var recipe = A.Recipe.Build();

        var filter = new RecipeFilter();

        foreach (var likedProduct in likedProducts)
        {
            preferences.LikeProduct(likedProduct.ProductId);
            recipe.AddIngredient(likedProduct);

            var product = new Product(likedProduct.ProductId, "a");

            var option = new ProductFilterOption(product, quantity);
            filter.AddOption(option);
        }

        var score = RecipePicker.ScoreRecipe(recipe, filter, preferences);

        Assert.AreEqual(280, score);
    }

    [Test]
    public void Test_LikedProductsWithoutOptions()
    {
        var likedProducts = new List<Ingredient>
        {
            new Ingredient(EntityId.NewId(), quantity),
            new Ingredient(EntityId.NewId(), quantity),
            new Ingredient(EntityId.NewId(), quantity)
        };

        var preferences = new Preferences();

        var recipe = A.Recipe.Build();

        var filter = new RecipeFilter();

        foreach (var likedProduct in likedProducts)
        {
            preferences.LikeProduct(likedProduct.ProductId);
            recipe.AddIngredient(likedProduct);
        }

        var score = RecipePicker.ScoreRecipe(recipe, filter, preferences);

        Assert.AreEqual(100, score);
    }

    [Test]
    public void Test_EnoughProducts()
    {
        var products = new List<Ingredient>
        {
            new Ingredient(EntityId.NewId(), quantity),
            new Ingredient(EntityId.NewId(), quantity),
            new Ingredient(EntityId.NewId(), quantity)
        };

        var preferences = new Preferences();

        var recipe = A.Recipe.Build();

        var filter = new RecipeFilter();

        foreach (var product in products)
        {
            recipe.AddIngredient(product);

            var actualProduct = new Product(product.ProductId, "a");

            var option = new ProductFilterOption(actualProduct, quantity);
            filter.AddOption(option);
        }

        var score = RecipePicker.ScoreRecipe(recipe, filter, preferences);

        Assert.AreEqual(130, score);
    }

    [Test]
    public void Test_NotEnoughProducts()
    {
        var products = new List<Ingredient>
        {
            new Ingredient(EntityId.NewId(), quantity),
            new Ingredient(EntityId.NewId(), quantity),
            new Ingredient(EntityId.NewId(), quantity)
        };

        var preferences = new Preferences();

        var recipe = A.Recipe.Build();

        var filter = new RecipeFilter();

        foreach (var product in products)
        {
            recipe.AddIngredient(product);

            var actualProduct = new Product(product.ProductId, "a");

            var option = new ProductFilterOption(actualProduct, lessQuantity);
            filter.AddOption(option);
        }

        var score = RecipePicker.ScoreRecipe(recipe, filter, preferences);

        Assert.AreEqual(-75, score);
    }
}
