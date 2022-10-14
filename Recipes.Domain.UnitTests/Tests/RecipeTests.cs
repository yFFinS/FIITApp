using NUnit.Framework;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.ValueObjects;
using Recipes.Tests.Shared.BuilderEntries;
using System;
using System.Collections.Generic;

namespace Recipes.Domain.UnitTests.Tests;

public class RecipeTests
{
    [Test]
    public void Test_NullTitle_Throw()
    {
#pragma warning disable CS8625
        var builder = A.Recipe.WithTitle(null);
#pragma warning restore CS8625
        Assert.Throws<ArgumentNullException>(() => builder.Build());
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(int.MinValue)]
    public void Test_InvalidServings_Throw(int servings)
    {
        var builder = A.Recipe
            .WithServings(servings);
        Assert.Throws<ArgumentException>(() => builder.Build());
    }

    [Test]
    public void Test_InvalidCookTime_Throw()
    {
        var builder = A.Recipe
            .WithCookingTime(TimeSpan.FromMinutes(-1));
        Assert.Throws<ArgumentException>(() => builder.Build());
    }

    [Test]
    public void Test_AddingNullIngredient_Throw()
    {
        Recipe recipe = A.Recipe;
#pragma warning disable CS8625
        Assert.Throws<ArgumentNullException>(() => recipe.AddIngredient(null));
#pragma warning restore CS8625
    }

    [Test]
    public void Test_RemovingNonexistentIngredient_ThrowCorrectException()
    {
        Ingredient ingredient = An.Ingredient;
        Recipe recipe = A.Recipe;

        Assert.Throws<IngredientNotFoundException>(() => recipe.RemoveIngredient(ingredient));
    }

    [Test]
    public void Test_AddingExistentIngredient_ThrowCorrectException()
    {
        Ingredient ingredient = An.Ingredient;
        Recipe recipe = A.Recipe.WithIngredient(ingredient);

        Assert.Throws<IngredientExistsException>(() => recipe.AddIngredient(ingredient));
    }

    [Test]
    public void Test_AddingIngredients_AddsInCorrectOrder()
    {
        Ingredient ingredient1 = An.Ingredient.WithProduct(A.Product.WithName("A"));
        Ingredient ingredient2 = An.Ingredient.WithProduct(A.Product.WithName("B"));
        Ingredient ingredient3 = An.Ingredient.WithProduct(A.Product.WithName("C"));

        Recipe recipe = A.Recipe;

        recipe.AddIngredient(ingredient1);
        recipe.AddIngredient(ingredient2);
        recipe.AddIngredient(ingredient3);

        var expected = new List<Ingredient> { ingredient1, ingredient2, ingredient3 };
        Assert.That(recipe.Ingredients, Is.EqualTo(expected));
    }

    [Test]
    public void Test_AddingCookingSteps_AddsInCorrectOrder()
    {
        var cookingStep1 = new CookingStep("1");
        var cookingStep2 = new CookingStep("2");
        var cookingStep3 = new CookingStep("3");

        Recipe recipe = A.Recipe;

        recipe.AddCookingStep(cookingStep1);
        recipe.AddCookingStep(cookingStep2);
        recipe.AddCookingStep(cookingStep3);

        var expected = new List<CookingStep> { cookingStep1, cookingStep2, cookingStep3 };
        Assert.That(recipe.CookingSteps, Is.EqualTo(expected));
    }

    [Test]
    public void Test_InsertingCookingSteps_InsertsInCorrectOrder()
    {
        var cookingStep1 = new CookingStep("1");
        var cookingStep2 = new CookingStep("2");
        var cookingStep3 = new CookingStep("3");

        Recipe recipe = A.Recipe;

        recipe.AddCookingStep(cookingStep1);
        recipe.AddCookingStep(cookingStep2);
        recipe.InsertCookingStep(0, cookingStep3);

        var expected = new List<CookingStep> { cookingStep3, cookingStep1, cookingStep2 };
        Assert.That(recipe.CookingSteps, Is.EqualTo(expected));
    }

    [Test]
    public void Test_UpdatingIngredient_UpdatesCorrectly()
    {
        var id = EntityId.NewId();

        Ingredient ingredient = An.Ingredient.WithProduct(A.Product.WithId(id));
        Recipe recipe = A.Recipe.WithIngredient(ingredient);

        Ingredient updatedIngredient = An.Ingredient.WithProduct(A.Product.WithId(id));
        recipe.UpdateIngredient(updatedIngredient);

        Assert.That(recipe.Ingredients, Has.Exactly(1)
            .With.Property(nameof(Ingredient.ProductId))
            .EqualTo(updatedIngredient.ProductId));
    }

    [Test]
    public void Test_EmptyRecipe_HasEmptyCookingSteps()
    {
        Recipe recipe = A.Recipe;
        Assert.That(recipe.CookingSteps, Is.Empty);
    }

    [Test]
    public void Test_EmptyRecipe_HasEmptyIngredients()
    {
        Recipe recipe = A.Recipe;
        Assert.That(recipe.Ingredients, Is.Empty);
    }
}