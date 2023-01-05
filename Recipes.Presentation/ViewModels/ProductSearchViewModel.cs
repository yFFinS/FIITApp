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
using Recipes.Presentation.Interfaces;

namespace Recipes.Presentation.ViewModels;

public class ProductSearchViewModel : ViewModelBase
{
    public IImageLoader ImageLoader { get; }
    public ObservableCollection<Product> Products { get; private set; }
    public HashSet<Product> SelectedProducts { get; set; }

    public ProductSearchViewModel(Lazy<IViewContainer> container, IImageLoader imageLoader,
        IProductRepository productRepository, IRecipePicker recipePicker)
    {
        ImageLoader = imageLoader;
        SearchCommand = ReactiveCommand.Create<string>(name => Search(name, productRepository));
        ShowRecipesCommand = ReactiveCommand.Create(() => ShowRecipes(container, imageLoader, productRepository, recipePicker));
        CheckProductCommand = ReactiveCommand.Create<Product>(CheckProduct);

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

    private async void ShowRecipes(Lazy<IViewContainer> container, IImageLoader loader, IProductRepository repository, IRecipePicker recipePicker)
    {
        var filter = new RecipeFilter();
        foreach (var product in SelectedProducts) 
            filter.AddOption(new ProductFilterOption(product));
        var recipes = await recipePicker.PickRecipes(filter);
        container.Value.Content = new RecipeListViewModel(recipes, container, loader, repository);
    }
}