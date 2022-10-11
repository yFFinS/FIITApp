using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Enums;
using Recipes.Domain.ValueObjects;

namespace Recipes.Presentation.ViewModels;

public class SearchViewModel : ViewModelBase
{
    private readonly Action<ViewModelBase> _setBaseContent;
    public SearchViewModel(IEnumerable<Recipe> items, Action<ViewModelBase> setContent)
    {
        Items = new ObservableCollection<Recipe>(items);
        _setBaseContent = setContent;
        ShowRecipeCommand = ReactiveCommand.Create<Recipe>(recipe => _setBaseContent(ShowRecipe(recipe)));
        Dispatcher.UIThread.InvokeAsync(() => { });
    }

    public ObservableCollection<Recipe> Items { get; }
    
    public ReactiveCommand<Recipe, Unit> ShowRecipeCommand { get; }

    public ViewModelBase ShowRecipe(Recipe recipe)
    {
        return new RecipeViewModel(recipe, () => _setBaseContent(this));
    }
}

internal class RecipesDataBase
{
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
        var result = new Recipe(EntityId.New(), name)
        {
            Description = $"{i} Description",
            CookDuration = new TimeSpan(i + 1,0,0,0),
            Servings = (i + 1) * 4
        };
        foreach (var ingredient in GetIngredients(i))
        {
            result.AddIngredient(ingredient);
        }
        result.AddCookingStep(new CookingStep($"{i % 3}"));
        i++;
        return result;
    }

    private static IEnumerable<Ingredient> GetIngredients(int i)
    {
        for (var j = 0; j < 10; j++)
        {
            var ingredient = new Ingredient(EntityId.New(), $"{j} ingr", new Quantity(j + 1, QuantityUnit.Milliliters));
            yield return ingredient;
            if (i > 3 && j == 9)
            {
                yield return new Ingredient(EntityId.New(), $"{i} ingred", new Quantity(1, QuantityUnit.Milliliters));
            }
        }

        if (i == 1)
        {
            yield return new Ingredient(EntityId.New(), $"{0} ingr", new Quantity(1, QuantityUnit.Milliliters));
        }
    }
}