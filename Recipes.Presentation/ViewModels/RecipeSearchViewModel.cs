using System;
using System.Collections.Generic;
using ReactiveUI;
using Recipes.Application.Interfaces;
using Recipes.Application.Services.RecipePicker;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Presentation.DataTypes;
using Recipes.Presentation.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Recipes.Domain.Interfaces;

namespace Recipes.Presentation.ViewModels;

public class RecipeSearchViewModel : ViewModelBase
{
    private readonly IRecipeRepository _recipeRepository;
    private List<ImageWrapper<Recipe>> _page;
    private string _searchPrefix;
    private int _pageIndex;

    public IImageLoader ImageLoader { get; }

    public ObservableCollection<ImageWrapper<Recipe>> Items { get; private set; }

    public string SearchPrefix
    {
        get => _searchPrefix;
        set => this.RaiseAndSetIfChanged(ref _searchPrefix, value);
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
        }
    }

    public int PageLastIndex => PageIndex + Page.Count;

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
        SearchPrefix = "";
    }

    private void ShowNextPage()
    {
        if (Items.Count - PageIndex <= 12) return;
        PageIndex += 12;
        Page = Enumerable.Range(PageIndex, Math.Min(12, Items.Count-PageIndex)).Select(i => Items[i]).ToList();
    }

    private void ShowPrevPage()
    {
        if (PageIndex < 12) return;
        PageIndex -= 12;
        Page = Enumerable.Range(PageIndex, 12).Select(i => Items[i]).ToList();
    }

    private ViewModelBase ShowRecipe(Recipe recipe, RecipeViewFactory factory)
    {
        return factory.Create(recipe, this);
    }

    public async void Search(string? prefix)
    {
        prefix ??= string.Empty;

        Items.Clear();
        var page = new List<ImageWrapper<Recipe>>();

        var index = 0;

        foreach (var recipe in await _recipeRepository.GetRecipesByPrefixAsync(prefix))
        {
            var item = new ImageWrapper<Recipe>(recipe, ImageLoader);
            Items.Add(item);
            if (index >= 12) continue;
            page.Add(item);
            index++;
        }

        Page = page;
        PageIndex = 0;
    }
}