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

    public IImageLoader ImageLoader { get; }

    public RecipeSearchViewModel(IViewContainer container, IRecipeRepository recipeRepository,
        RecipeViewFactory factory,
        IExceptionContainer exceptionContainer, IImageLoader imageLoader)
    {
        Items = new List<ImageWrapper<Recipe>>();
        Page = new List<ImageWrapper<Recipe>>();
        _recipeRepository = recipeRepository;
        ImageLoader = imageLoader;
        Search(null);
        ShowRecipeCommand = ReactiveCommandExtended.Create<Recipe>(recipe =>
            container.Content = ShowRecipe(recipe, factory), exceptionContainer);
        SearchCommand = ReactiveCommandExtended.Create<string>(Search, exceptionContainer);
    }

    public List<ImageWrapper<Recipe>> Items { get; private set; }

    public List<ImageWrapper<Recipe>> Page
    {
        get => _page;
        set => this.RaiseAndSetIfChanged(ref _page, value);
    }

    public ReactiveCommand<Recipe, Unit> ShowRecipeCommand { get; }
    public ReactiveCommand<string, Unit> SearchCommand { get; }

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
    }
}