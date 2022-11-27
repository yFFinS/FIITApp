using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Threading;
using ReactiveUI;
using Recipes.Application.Interfaces;
using Recipes.Application.Services.RecipePicker;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Enums;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;
using Recipes.Presentation.Interfaces;

namespace Recipes.Presentation.ViewModels;

public class RecipeSearchViewModel : ViewModelBase
{
    private readonly IRecipePicker _picker;

    public RecipeSearchViewModel(Lazy<IViewContainer> container, IImageLoader imageLoader, IRecipePicker picker)
    {
        Items = new ObservableCollection<Recipe>(Enumerable.Empty<Recipe>());
        _picker = picker;
        ShowRecipeCommand = ReactiveCommand.Create<Recipe>(recipe =>
            container.Value.Content = ShowRecipe(recipe, container, imageLoader));
        SearchCommand = ReactiveCommand.Create<string>(Search);
        Dispatcher.UIThread.InvokeAsync(() => { });
    }

    public ObservableCollection<Recipe> Items { get; private set; }
    
    public ReactiveCommand<Recipe, Unit> ShowRecipeCommand { get; }
    public ReactiveCommand<string, Unit> SearchCommand { get; }

    private ViewModelBase ShowRecipe(Recipe recipe, Lazy<IViewContainer> container, IImageLoader loader)
    {
        return new RecipeViewModel(recipe, container, loader, this);
    }

    public async void Search(string? name)
    {
        var filter = new RecipeFilter()
        {
            MaxRecipes = 24
        };
        Items.Clear();
        foreach (var recipe in await _picker.PickRecipes(filter))
        {
            Items.Add(recipe);
        }
    }
}

internal class RecipeRepository : IRecipeRepository
{
    public Task<List<Recipe>> GetRecipesAsync()
    {
        return Task.Run(() => new List<Recipe>(Items));
    }

    public IEnumerable<Recipe> Items
    {
        get
        {
            for (var i = 0; i < 12; i++)
            {
                yield return RecipeFactory.GetRecipe("Apple");
            }
        }
    }
}

internal static class RecipeFactory
{
    private static int i = 0;
    public static Recipe GetRecipe(string name)
    {
        var result = new Recipe(EntityId.NewId(), name,
            string.Join(";", Enumerable.Repeat($"{i} Description", 20)),
            (i + 1) * 4,
            new TimeSpan(i + 1, 0, 0, 0));
        foreach (var ingredient in GetIngredients(i))
        {
            result.AddIngredient(ingredient);
        }
        result.AddCookingStep(new CookingStep($"{i % 3}"));
        for (int j = 0; j < 10; j++)
        {
            result.AddCookingStep(new CookingStep(String.Join(" ", Enumerable.Repeat($"{j}", 100))));
        }
        i++;
        return result;
    }

    private static IEnumerable<Ingredient> GetIngredients(int i)
    {
        for (var j = 0; j < 10; j++)
        {
            var ingredient = new Ingredient(EntityId.NewId(), new Quantity(j + 1, QuantityUnit.Milliliters));
            yield return ingredient;
            if (i > 3 && j == 9)
            {
                yield return new Ingredient(EntityId.NewId(), new Quantity(1, QuantityUnit.Milliliters));
            }
        }

        if (i == 1)
        {
            yield return new Ingredient(EntityId.NewId(), new Quantity(1, QuantityUnit.Milliliters));
        }
    }
}