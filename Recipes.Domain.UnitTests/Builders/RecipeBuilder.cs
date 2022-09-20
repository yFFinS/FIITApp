using System;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.UnitTests.Builders;

public class RecipeBuilder
{
    private EntityId _id = EntityId.New();
    private string _title = "Test Recipe";
    private string? _description;
    private int _servings = 1;
    private TimeSpan _cookTime = TimeSpan.FromHours(1);
    private Ingredients _ingredients = new();
    private CookingTechnic _cookingTechnic = new();

    public RecipeBuilder WithId(EntityId id)
    {
        _id = id;
        return this;
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

    public Recipe Build()
    {
        var recipe = new Recipe(_id, _title, _description, _servings, _cookTime);
        foreach (var ingredient in _ingredients)
        {
            recipe.AddIngredient(ingredient);
        }

        foreach (var step in _cookingTechnic.CookingSteps)
        {
            recipe.AddCookingStep(step);
        }

        return recipe;
    }

    public static implicit operator Recipe(RecipeBuilder builder) => builder.Build();
}