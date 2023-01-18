using System;
using ReactiveUI;
using Recipes.Application.Interfaces;
using Recipes.Application.Services.RecipePicker;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Presentation.DataTypes;
using Recipes.Presentation.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;

namespace Recipes.Presentation.ViewModels;

public class ProductSearchViewModel : ViewModelBase
{
    private string _searchPrefix;
    private List<ImageWrapper<Product>> _products;
    private int _pageIndex;
    private List<ImageWrapper<Product>> _page;
    public IImageLoader ImageLoader { get; }
    public IProductRepository ProductRepository { get; }

    public List<ImageWrapper<Product>> Products
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
        ShowNextPageCommand = ReactiveCommandExtended.Create(ShowNextPage, exceptionContainer);
        ShowPreviousPageCommand = ReactiveCommandExtended.Create(ShowPrevPage, exceptionContainer);

        Products = new List<ImageWrapper<Product>>();
        SelectedProducts = new HashSet<Product>();
    }

    public string SearchPrefix
    {
        get => _searchPrefix;
        set => this.RaiseAndSetIfChanged(ref _searchPrefix, value);
    }
    
    public List<ImageWrapper<Product>> Page
    {
        get => _page;
        set => this.RaiseAndSetIfChanged(ref _page, value);
    }
    
    public int PageIndex
    {
        get => _pageIndex;
        set
        {
            this.RaiseAndSetIfChanged(ref _pageIndex, value);
            this.RaisePropertyChanged("PageLastIndex");
        }
    }

    public int PageLastIndex => PageIndex + Page.Count;

    public ReactiveCommand<Unit, Unit> ShowRecipesCommand { get; }
    public ReactiveCommand<string, Unit> SearchCommand { get; }
    public ReactiveCommand<Product, Unit> CheckProductCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowNextPageCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowPreviousPageCommand { get; }

    public override void Refresh()
    {
        Search(null);
        SearchPrefix = "";
        SelectedProducts.Clear();
    }

    private void Search(string? prefix)
    {
        Products.Clear();
        var page = new List<ImageWrapper<Product>>();

        var index = 0;
        foreach (var product in string.IsNullOrWhiteSpace(prefix)
                     ? ProductRepository.GetAllProducts()
                     : ProductRepository.GetProductsByPrefix(prefix))
        {
            var item = new ImageWrapper<Product>(product, ImageLoader, product.ImageUrl);
            Products.Add(item);
            if(index >= PageCapacity) continue;
            page.Add(item);
            index++;
        }

        Page = page;
        PageIndex = 0;
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

    private const int PageCapacity = 16;
    
    private void ShowNextPage()
    {
        if (Products.Count - PageIndex <= PageCapacity) return;
        PageIndex += PageCapacity;
        Page = Enumerable.Range(PageIndex, Math.Min(PageCapacity, Products.Count - PageIndex)).Select(i => Products[i])
            .ToList();
    }

    private void ShowPrevPage()
    {
        if (PageIndex < PageCapacity) return;
        PageIndex -= PageCapacity;
        Page = Enumerable.Range(PageIndex, PageCapacity).Select(i => Products[i]).ToList();
    }
}