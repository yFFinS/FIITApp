using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
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
using Recipes.Presentation.DataTypes;
using Recipes.Presentation.Interfaces;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace Recipes.Presentation.ViewModels;

public class ProductSearchViewModel : ViewModelBase
{
    public IImageLoader ImageLoader { get; }
    public ObservableCollection<Product> Products { get; private set; }
    public HashSet<Product> SelectedProducts { get; set; }

    public ProductSearchViewModel(IViewContainer container, IImageLoader imageLoader,
        IProductRepository productRepository, IRecipePicker recipePicker, RecipeViewFactory factory, IExceptionContainer exceptionContainer)
    {
        ImageLoader = imageLoader;
        SearchCommand =
            ReactiveCommandExtended.Create<string>(name => Search(name, productRepository), exceptionContainer);
        ShowRecipesCommand =
            ReactiveCommandExtended.Create(() => ShowRecipes(container, imageLoader, recipePicker, factory, exceptionContainer),
                exceptionContainer);
        CheckProductCommand = ReactiveCommandExtended.Create<Product>(CheckProduct, exceptionContainer);

        Products = new ObservableCollection<Product>(Enumerable.Empty<Product>());
        SelectedProducts = new HashSet<Product>();
    }

    public ReactiveCommand<Unit, Unit> ShowRecipesCommand { get; }
    public ReactiveCommand<string, Unit> SearchCommand { get; }
    public ReactiveCommand<Product, Unit> CheckProductCommand { get; }

    private async void Search(string name, IProductRepository repository)
    {
        Products.Clear();
        foreach (var product in await repository.GetProductsByPrefixAsync(name))
        {
            Products.Add(product);
        }
    }

    private void CheckProduct(Product product)
    {
        if (SelectedProducts.Contains(product))
            SelectedProducts.Remove(product);
        else
            SelectedProducts.Add(product);
    }

    private async void ShowRecipes(IViewContainer container, IImageLoader loader, IRecipePicker recipePicker,
        RecipeViewFactory factory, IExceptionContainer exceptionContainer)
    {
        var filter = new RecipeFilter();
        foreach (var product in SelectedProducts) 
            filter.AddOption(new ProductFilterOption(product));
        var recipes = await recipePicker.PickRecipes(filter);
        container.Content = new RecipeListViewModel(recipes, container, loader, factory, exceptionContainer);
    }
}