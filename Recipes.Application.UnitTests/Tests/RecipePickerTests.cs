using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Recipes.Application.Services.RecipePicker;
using Recipes.Application.Services.RecipePicker.ScoringCriteria;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;
using Recipes.Tests.Shared.BuilderEntries;

namespace Recipes.Application.UnitTests.Tests;

public class FakeRecipeRepository : IRecipeRepository
{
    private readonly List<Recipe> _recipes;

    public FakeRecipeRepository(IEnumerable<Recipe> recipes)
    {
        _recipes = recipes.ToList();
    }

    public List<Recipe> GetAllRecipes(bool onlyGlobal = false)
    {
        return _recipes.ToList();
    }

    public Recipe? GetRecipeById(EntityId recipeId) => throw new NotImplementedException();

    public Recipe? GetRecipeByName(string recipeName) => throw new NotImplementedException();

    public List<Recipe> GetRecipesBySubstring(string substring) => throw new NotImplementedException();

    public void AddRecipes(IEnumerable<Recipe> recipes, bool useUserDatabase = false) =>
        throw new NotImplementedException();

    public void RemoveRecipesById(IEnumerable<EntityId> recipeIds) => throw new NotImplementedException();
}

[TestFixture]
public class RecipePickerTests
{
    [Test]
    public void Test_PickBestRecipes_And_DoNotPickBadRecipes()
    {
        Product mainProduct1 = A.Product.WithName("Main1");
        Product mainProduct2 = A.Product.WithName("Main2");
        Product otherProduct = A.Product.WithName("Other");

        Ingredient mainIngredient1 = An.Ingredient.WithProduct(mainProduct1);
        Ingredient mainIngredient2 = An.Ingredient.WithProduct(mainProduct2);
        Ingredient otherIngredient = An.Ingredient.WithProduct(otherProduct);

        var recipes = new List<Recipe>()
        {
            A.Recipe
                .WithIngredient(mainIngredient1)
                .WithIngredient(mainIngredient2)
                .WithIngredient(otherIngredient),
            A.Recipe
                .WithIngredient(mainIngredient1)
                .WithIngredient(otherIngredient),
            A.Recipe
                .WithIngredient(otherIngredient),
        };

        var fakeRepository = new FakeRecipeRepository(recipes);
        var criteria = new FilterScoringCriteria(new FilterCriteriaScores(-10,
            10, 100, -15, -1000));
        var picker = new RecipePicker(NullLogger<RecipePicker>.Instance, fakeRepository, new[] { criteria });

        var filter = new RecipeFilter();
        filter.AddOption(new ProductFilterOption(mainProduct1));
        filter.MaxRecipes = recipes.Count;

        var pickedRecipes = picker.PickRecipes(filter);

        Assert.That(pickedRecipes.Count, Is.EqualTo(2));
        Assert.AreEqual(recipes[0], pickedRecipes[0]);
        Assert.AreEqual(recipes[1], pickedRecipes[1]);
    }
}