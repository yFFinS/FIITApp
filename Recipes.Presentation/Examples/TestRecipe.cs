using System;
using JetBrains.Annotations;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Enums;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Presentation.Examples;

public class TestRecipe : Recipe
{
    public TestRecipe() : base(EntityId.NewId(), "Apple Juice", new EnergyValue(0, 0, 0, 0))
    {
        Description = "Juice of fresh apples";
        Servings = 5;
        CookDuration = new TimeSpan(0, 1, 0, 0);
        ImageUrl = new Uri("https://www.goodnature.com/wp-content/uploads/2021/07/apple-juice-hero.jpg");

        var pieces = new QuantityUnit("штуки", "шт");
        AddIngredient(new Ingredient(EntityId.NewId(), new Quantity(10, pieces)));
        AddCookingStep(new CookingStep("1. Steel juice from first apple"));
        AddCookingStep(new CookingStep("2. Steel juice from second apple"));
        AddCookingStep(new CookingStep("3. Steel juice from third apple"));
        AddCookingStep(new CookingStep("4. Steel juice from fourth apple"));
        AddCookingStep(new CookingStep("5. Steel juice from fifth apple"));
        AddCookingStep(new CookingStep("6. Steel juice from sixth apple"));
        AddCookingStep(new CookingStep("7. Steel juice from seventh apple"));
        AddCookingStep(new CookingStep("8. Steel juice from 8th apple"));
        AddCookingStep(new CookingStep("9. Steel juice from 9th apple"));
        AddCookingStep(new CookingStep("10. Steel juice from 10th apple"));
    }
}