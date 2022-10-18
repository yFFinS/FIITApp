﻿using System;
using System.Reactive;
using ReactiveUI;
using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Presentation.ViewModels;

public class RecipeViewModel : ViewModelBase
{
    public Recipe Recipe { get; set; }

    public ReactiveCommand<Unit, Unit> BackCommand { get; }

    public RecipeViewModel(Recipe recipe, Action back)
    {
        Recipe = recipe;
        BackCommand = ReactiveCommand.Create(back);
    }
}