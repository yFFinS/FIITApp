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
using Recipes.Domain.ValueObjects;

namespace Recipes.Presentation.ViewModels;

public class SearchViewModel : ViewModelBase
{
    public SearchViewModel(IEnumerable<Recipe> items, Action<ViewModelBase> setContent)
    {
        Items = new ObservableCollection<Recipe>(items);
        ShowRecipeCommand = ReactiveCommand.Create<Recipe>(recipe => setContent(ShowRecipe(recipe)));
        Dispatcher.UIThread.InvokeAsync(() => { });
    }

    public ObservableCollection<Recipe> Items { get; }
    
    public ReactiveCommand<Recipe, Unit> ShowRecipeCommand { get; }

    public ViewModelBase ShowRecipe(Recipe recipe)
    {
        return new RecipeViewModel(recipe);
    }
}

internal class RecipesDataBase
{
    public IEnumerable<Recipe> Items => Enumerable.Repeat(new Recipe(EntityId.New(), "Apple"), 100);
}