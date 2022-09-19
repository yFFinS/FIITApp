using System;
using NUnit.Framework;
using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Domain.UnitTests;

public class RecipeTests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase(null, null, 0, null, null)]
    public void TestInvalidParameters(string name, string? description, int servings,
        Ingredients ingredients, CookingTechnic cookingTechnic)
    {
        var cookDuration = TimeSpan.FromHours(1);
        Assert.Throws<ArgumentException>(() =>
            new Recipe(name, description, servings, cookDuration, ingredients, cookingTechnic));
    }
}