using System;
using System.Reactive;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging.Abstractions;
using ReactiveUI;
using Recipes.Application.Services.Preferences;
using Recipes.Application.Services.RecipePicker;
using Recipes.Presentation.Interfaces;

namespace Recipes.Presentation.ViewModels;

public class SelectionViewModel : ViewModelBase
{
    private readonly SearchViewModel _standardSearch;
    
    public SelectionViewModel(Lazy<IViewContainer> container, SearchViewModel standardSearch)
    {
        _standardSearch = standardSearch;
        ShowStandardSearch = ReactiveCommand.Create(() =>
        {
            container.Value.Content = _standardSearch;
        });
    }

    public ReactiveCommand<Unit, Unit> ShowStandardSearch { get; }
}