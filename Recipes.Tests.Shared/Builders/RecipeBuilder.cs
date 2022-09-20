using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Tests.Shared.Builders;

public class RecipeBuilder : BaseEntityBuilder<Recipe, RecipeBuilder>
{
    private string _title = "Test Recipe";
    private string? _description;
    private int _servings = 1;
    private TimeSpan _cookTime = TimeSpan.FromHours(1);
    private IngredientGroup _ingredientGroup = new();
    private CookingTechnic _cookingTechnic = new();

    protected override IEnumerable<object?> GetConstructorArguments()
    {
        yield return _title;
        yield return _description;
        yield return _servings;
        yield return _cookTime;
        yield return _ingredientGroup;
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

    public RecipeBuilder WithIngredients(IngredientGroup ingredientGroup)
    {
        _ingredientGroup = ingredientGroup;
        return this;
    }

    public RecipeBuilder WithIngredient(Ingredient ingredient)
    {
        _ingredientGroup.Add(ingredient);
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