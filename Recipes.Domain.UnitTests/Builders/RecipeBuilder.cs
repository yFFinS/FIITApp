using System;
using System.Collections.Generic;
using Recipes.Domain.Entities.IngredientAggregate;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.UnitTests.Builders;

public class RecipeBuilder : BaseEntityBuilder<Recipe, RecipeBuilder>
{
    private string _title = "Test Recipe";
    private string? _description;
    private int _servings = 1;
    private TimeSpan _cookTime = TimeSpan.FromHours(1);
    private Ingredients _ingredients = new();
    private CookingTechnic _cookingTechnic = new();

    protected override IEnumerable<object?> GetConstructorArguments()
    {
        yield return _title;
        yield return _description;
        yield return _servings;
        yield return _cookTime;
        yield return _ingredients;
        yield return _cookingTechnic;
    }

    public RecipeBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public RecipeBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public RecipeBuilder WithoutDescription()
    {
        return WithDescription(null);
    }

    public RecipeBuilder WithServings(int servings)
    {
        _servings = servings;
        return this;
    }

    public RecipeBuilder WithCookingTime(TimeSpan cookingTime)
    {
        _cookTime = cookingTime;
        return this;
    }

    public RecipeBuilder WithIngredients(Ingredients ingredients)
    {
        _ingredients = ingredients;
        return this;
    }

    public RecipeBuilder WithIngredient(Ingredient ingredient)
    {
        _ingredients.Add(ingredient);
        return this;
    }

    public RecipeBuilder WithCookingTechnic(CookingTechnic cookingTechnic)
    {
        _cookingTechnic = cookingTechnic;
        return this;
    }

    public RecipeBuilder WithCookingStep(CookingStep cookingStep)
    {
        _cookingTechnic.AddStep(cookingStep);
        return this;
    }
}