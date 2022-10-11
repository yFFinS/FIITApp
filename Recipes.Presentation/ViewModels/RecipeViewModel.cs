using System;
using System.Reactive;
using ReactiveUI;
using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Presentation.ViewModels;

public class RecipeViewModel : ViewModelBase
{
    public Recipe Recipe { get; set; }

    public readonly Action BackCommand;

    public RecipeViewModel(Recipe recipe, Action back)
    {
        Recipe = recipe;
        BackCommand = back;
    }

    public void Back() => BackCommand();
}