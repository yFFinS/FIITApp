using System;
using System.Reactive;
using ReactiveUI;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Presentation.Interfaces;

namespace Recipes.Presentation.ViewModels;

public class RecipeViewModel : ViewModelBase
{
    public Recipe Recipe { get; set; }
    public ReactiveCommand<Unit, Unit> BackCommand { get; }
    

    public RecipeViewModel(Recipe recipe, Lazy<IViewContainer> container, ViewModelBase parent)
    {
        Recipe = recipe;
        BackCommand = ReactiveCommand.Create(() =>
        {
            container.Value.Content = parent;
        });
    }
}