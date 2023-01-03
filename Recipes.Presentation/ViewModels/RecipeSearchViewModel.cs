using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using Recipes.Application.Interfaces;
using Recipes.Application.Services.RecipePicker;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Presentation.Interfaces;

namespace Recipes.Presentation.ViewModels;

public class RecipeSearchViewModel : ViewModelBase
{
    private readonly IRecipePicker _picker;

    public RecipeSearchViewModel(Lazy<IViewContainer> container, IImageLoader imageLoader, IProductRepository repository, IRecipePicker picker)
    {
        Items = new ObservableCollection<Recipe>(Enumerable.Empty<Recipe>());
        _picker = picker;
        ShowRecipeCommand = ReactiveCommand.Create<Recipe>(recipe =>
            container.Value.Content = ShowRecipe(recipe, container, imageLoader, repository));
        SearchCommand = ReactiveCommand.Create<string>(Search);
    }

    public ObservableCollection<Recipe> Items { get; private set; }
    
    public ReactiveCommand<Recipe, Unit> ShowRecipeCommand { get; }
    public ReactiveCommand<string, Unit> SearchCommand { get; }

    private ViewModelBase ShowRecipe(Recipe recipe, Lazy<IViewContainer> container, IImageLoader loader, IProductRepository repository)
    {
        return new RecipeViewModel(recipe, container, loader, repository, this);
    }

    public async void Search(string? name)
    {
        var filter = new RecipeFilter()
        {
            MaxRecipes = 24
        };
        Items.Clear();
        foreach (var recipe in await _picker.PickRecipes(filter))
        {
            Items.Add(recipe);
        }
    }
}