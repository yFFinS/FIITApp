using ReactiveUI;
using Recipes.Application.Interfaces;
using Recipes.Application.Services.RecipePicker;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Presentation.DataTypes;
using Recipes.Presentation.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace Recipes.Presentation.ViewModels;

public class ProductSearchViewModel : ViewModelBase
{
    private string _searchPrefix;
    private List<Product> _products;
    public IImageLoader ImageLoader { get; }
    public IProductRepository ProductRepository { get; }

    public List<Product> Products
    {
        get => _products;
        set => this.RaiseAndSetIfChanged(ref _products, value);
    }

    public HashSet<Product> SelectedProducts { get; set; }

    public ProductSearchViewModel(IViewContainer container, IImageLoader imageLoader,
        IProductRepository productRepository, IRecipePicker recipePicker, RecipeViewFactory factory,
        IExceptionContainer exceptionContainer)
    {
        ImageLoader = imageLoader;
        ProductRepository = productRepository;
        SearchCommand =
            ReactiveCommandExtended.Create<string>(Search, exceptionContainer);
        ShowRecipesCommand =
            ReactiveCommandExtended.Create(
                () => ShowRecipes(container, imageLoader, recipePicker, factory, exceptionContainer),
                exceptionContainer);
        CheckProductCommand = ReactiveCommandExtended.Create<Product>(CheckProduct, exceptionContainer);

        Products = new List<Product>();
        SelectedProducts = new HashSet<Product>();
    }

    public string SearchPrefix
    {
        get => _searchPrefix;
        set => this.RaiseAndSetIfChanged(ref _searchPrefix, value);
    }

    public ReactiveCommand<Unit, Unit> ShowRecipesCommand { get; }
    public ReactiveCommand<string, Unit> SearchCommand { get; }
    public ReactiveCommand<Product, Unit> CheckProductCommand { get; }

    public override void Refresh()
    {
        Search(null);
        SearchPrefix = "";
    }

    private void Search(string? prefix)
    {
        Products = string.IsNullOrWhiteSpace(prefix)
            ? ProductRepository.GetAllProducts()
            : ProductRepository.GetProductsByPrefix(prefix);
    }

    private void CheckProduct(Product product)
    {
        if (SelectedProducts.Contains(product))
            SelectedProducts.Remove(product);
        else
            SelectedProducts.Add(product);
    }

    private void ShowRecipes(IViewContainer container, IImageLoader loader, IRecipePicker recipePicker,
        RecipeViewFactory factory, IExceptionContainer exceptionContainer)
    {
        var filter = new RecipeFilter
        {
            MaxRecipes = 24
        };
        foreach (var product in SelectedProducts)
            filter.AddOption(new ProductFilterOption(product));
        var recipes = recipePicker.PickRecipes(filter);
        container.Content = new RecipeListViewModel(recipes, container, loader, factory, exceptionContainer);
    }
}