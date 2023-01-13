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
    public IImageLoader ImageLoader { get; }
    public IProductRepository ProductRepository { get; }
    public ObservableCollection<Product> Products { get; private set; }
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
            ReactiveCommandExtended.Create(() => ShowRecipes(container, imageLoader, recipePicker, factory, exceptionContainer),
                exceptionContainer);
        CheckProductCommand = ReactiveCommandExtended.Create<Product>(CheckProduct, exceptionContainer);

        Products = new ObservableCollection<Product>(Enumerable.Empty<Product>());
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

    private async void Search(string? prefix)
    {
        prefix ??= string.Empty;
        Products.Clear();
        foreach (var product in await ProductRepository.GetProductsByPrefixAsync(prefix))
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
        filter.MaxRecipes = 24;
        foreach (var product in SelectedProducts)
            filter.AddOption(new ProductFilterOption(product));
        var recipes = await recipePicker.PickRecipes(filter);
        container.Content = new RecipeListViewModel(recipes, container, loader, factory, exceptionContainer);
    }
}