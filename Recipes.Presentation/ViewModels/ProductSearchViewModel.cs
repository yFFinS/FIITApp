using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging.Abstractions;
using ReactiveUI;
using Recipes.Application.Interfaces;
using Recipes.Application.Services.Preferences;
using Recipes.Application.Services.RecipePicker;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.ValueObjects;
using Recipes.Presentation.Interfaces;

namespace Recipes.Presentation.ViewModels;

public class ProductSearchViewModel : ViewModelBase
{
    
    public ObservableCollection<Product> Products { get; private set; }

    public ProductSearchViewModel(Lazy<IViewContainer> container, IImageLoader loader,
        IProductRepository productRepository)
    {
        SearchCommand = ReactiveCommand.Create<string>(name => Search(name, productRepository));
        ShowRecipesCommand = ReactiveCommand.Create(() => ShowRecipes(container, loader));

        Products = new ObservableCollection<Product>(Enumerable.Empty<Product>());
    }

    public ReactiveCommand<Unit, Unit> ShowRecipesCommand { get; }
    
    public ReactiveCommand<string, Unit> SearchCommand { get; }

    private async void Search(string name, IProductRepository repository)
    {
        Products.Clear();
        foreach (var product in await repository.GetAllProductsAsync())
        {
            Products.Add(product);
        }
    }

    private void ShowRecipes(Lazy<IViewContainer> container, IImageLoader loader)
    {
        //todo findrecipes by products
        var recipes = new List<Recipe>
        {
            new(EntityId.NewId(), "Apple"),
            new(EntityId.NewId(), "Apple")
        };
        container.Value.Content = new RecipeListViewModel(recipes, container, loader);
    }
}