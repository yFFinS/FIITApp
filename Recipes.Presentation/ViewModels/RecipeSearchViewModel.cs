using ReactiveUI;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Interfaces;
using Recipes.Presentation.DataTypes;
using Recipes.Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace Recipes.Presentation.ViewModels;

public class RecipeSearchViewModel : ViewModelBase
{
    private readonly IRecipeRepository _recipeRepository;
    private List<ImageWrapper<Recipe>> _page;
    private string _searchSubstring;
    private int _pageIndex;

    public IImageLoader ImageLoader { get; }

    public ObservableCollection<ImageWrapper<Recipe>> Items { get; private set; }

    public string SearchSubstring
    {
        get => _searchSubstring;
        set => this.RaiseAndSetIfChanged(ref _searchSubstring, value);
    }

    public List<ImageWrapper<Recipe>> Page
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
            this.RaisePropertyChanged("PageFirstIndex");
        }
    }

    public int PageLastIndex => Math.Max(PageIndex + Page.Count, 1);
    public int PageFirstIndex => PageIndex + 1;

    public ReactiveCommand<Recipe, Unit> ShowRecipeCommand { get; }
    public ReactiveCommand<string, Unit> SearchCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowNextPageCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowPreviousPageCommand { get; }

    public RecipeSearchViewModel(IViewContainer container, IRecipeRepository recipeRepository,
        RecipeViewFactory factory,
        IExceptionContainer exceptionContainer, IImageLoader imageLoader)
    {
        Items = new ObservableCollection<ImageWrapper<Recipe>>();
        Page = new List<ImageWrapper<Recipe>>();
        _recipeRepository = recipeRepository;
        ImageLoader = imageLoader;
        ShowRecipeCommand = ReactiveCommandExtended.Create<Recipe>(recipe =>
            container.Content = ShowRecipe(recipe, factory), exceptionContainer);
        SearchCommand = ReactiveCommandExtended.Create<string>(Search, exceptionContainer);
        ShowNextPageCommand = ReactiveCommandExtended.Create(ShowNextPage, exceptionContainer);
        ShowPreviousPageCommand = ReactiveCommandExtended.Create(ShowPrevPage, exceptionContainer);
    }

    public override void Refresh()
    {
        Search(null);
        SearchSubstring = "";
    }

    private const int PageCapacity = 12;

    private void ShowNextPage()
    {
        if (Items.Count - PageIndex <= PageCapacity) return;
        PageIndex += PageCapacity;
        Page = Enumerable.Range(PageIndex, Math.Min(PageCapacity, Items.Count - PageIndex)).Select(i => Items[i])
            .ToList();
        this.RaisePropertyChanged("PageLastIndex");
    }

    private void ShowPrevPage()
    {
        if (PageIndex < PageCapacity) return;
        PageIndex -= PageCapacity;
        Page = Enumerable.Range(PageIndex, PageCapacity).Select(i => Items[i]).ToList();
        this.RaisePropertyChanged("PageLastIndex");
    }

    private ViewModelBase ShowRecipe(Recipe recipe, RecipeViewFactory factory)
    {
        return factory.Create(recipe, this);
    }

    public void Search(string? substring)
    {
        substring ??= string.Empty;

        Items.Clear();
        var page = new List<ImageWrapper<Recipe>>();

        var index = 0;

        foreach (var item in _recipeRepository.GetRecipesBySubstring(substring)
                     .Select(recipe => new ImageWrapper<Recipe>(recipe, ImageLoader, recipe.ImageUrl)))
        {
            Items.Add(item);
            if (index >= 12) continue;
            page.Add(item);
            index++;
        }

        Page = page;
        PageIndex = 0;
    }
}