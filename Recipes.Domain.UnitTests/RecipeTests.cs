using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using NUnit.Framework;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Enums;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.UnitTests;

public interface IRecipeBuilder
{
    IRecipeBuilder WithValidFields();
    IRecipeBuilder WithTitle(string title);
    IRecipeBuilder WithDescription(string? description);
    IRecipeBuilder WithCookingTime(TimeSpan cookingTime);
    IRecipeBuilder WithIngredients(Ingredients ingredients);
    IRecipeBuilder WithIngredient(Ingredient ingredient);
    IRecipeBuilder WithCookingTechnic(CookingTechnic cookingTechnic);
    IRecipeBuilder WithCookingStep(CookingStep cookingStep);
}

public static class RecipeBuilderExtensions
{
    public static IRecipeBuilder WithoutDescription(this IRecipeBuilder builder)
    {
        return builder.WithDescription(null);
    }
}

public class RecipeBuilder : IRecipeBuilder
{
    private readonly Recipe _recipe = new();

    public IRecipeBuilder WithValidFields()
    {
        _recipe.Title = "Default Recipe";
        _recipe.Description = "Default Description";
        return this;
    }

    public IRecipeBuilder WithTitle(string title)
    {
        _recipe.Title = title;
        return this;
    }

    public IRecipeBuilder WithDescription(string? description)
    {
        _recipe.Description = description;
        return this;
    }

    public IRecipeBuilder WithCookingTime(TimeSpan cookingTime)
    {
        _recipe.CookDuration = cookingTime;
        return this;
    }

    public IRecipeBuilder WithIngredients(Ingredients ingredients)
    {
        _recipe.Ingredients = ingredients;
        return this;
    }

    public IRecipeBuilder WithIngredient(Ingredient ingredient)
    {
        _recipe.Ingredients.Add(ingredient);
        return this;
    }

    public IRecipeBuilder WithCookingTechnic(CookingTechnic cookingTechnic)
    {
        _recipe.CookingTechnic = cookingTechnic;
        return this;
    }

    public IRecipeBuilder WithCookingStep(CookingStep cookingStep)
    {
        _recipe.CookingTechnic.AddCookingStep(cookingStep);
        return this;
    }

    public Recipe Build()
    {
        return _recipe;
    }
}

public static class A
{
    public static IRecipeBuilder Recipe => new RecipeBuilder();
}

public class RecipeTests
{
    [SetUp]
    public void Setup()
    {
    }

    public void TestInvalidTitles(string title)
    {
    }

    public void AssertInvalidConstructor(string title, string? description, int servings,
        TimeSpan cookDuration, Ingredients ingredients, CookingTechnic cookingTechnic)
    {
        Assert.Throws<ArgumentException>(() =>
            new Recipe(title, description, servings, cookDuration, ingredients, cookingTechnic));
    }
}