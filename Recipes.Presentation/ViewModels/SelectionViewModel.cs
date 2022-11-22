using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging.Abstractions;
using ReactiveUI;
using Recipes.Application.Services.Preferences;
using Recipes.Application.Services.RecipePicker;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Presentation.Interfaces;

namespace Recipes.Presentation.ViewModels;

public class SelectionViewModel : ViewModelBase
{
    private readonly SearchViewModel _standardSearch;
    
    public ObservableCollection<Product> Products { get; private set; }

    public SelectionViewModel(Lazy<IViewContainer> container, SearchViewModel standardSearch)
    {
        _standardSearch = standardSearch;
        ShowStandardSearch = ReactiveCommand.Create(() =>
        {
            container.Value.Content = _standardSearch;
        });

        Products = new ObservableCollection<Product>(Enumerable.Empty<Product>());
    }

    public ReactiveCommand<Unit, Unit> ShowStandardSearch { get; }
}