using ReactiveUI;
using Recipes.Application.Interfaces;
using Recipes.Application.Services.RecipePicker;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Presentation.DataTypes;
using Recipes.Presentation.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace Recipes.Presentation.ViewModels;

public class RecipeSearchViewModel : ViewModelBase
{
    private readonly IRecipePicker _picker;

    public IImageLoader ImageLoader { get; }

    public RecipeSearchViewModel(IViewContainer container, IRecipePicker picker, RecipeViewFactory factory,
        IExceptionContainer exceptionContainer, IImageLoader imageLoader)
    {
        Items = new ObservableCollection<Recipe>(Enumerable.Empty<Recipe>());
        _picker = picker;
        ImageLoader = imageLoader;
        Search("");
        ShowRecipeCommand = ReactiveCommandExtended.Create<Recipe>(recipe =>
            container.Content = ShowRecipe(recipe, factory), exceptionContainer);
        SearchCommand = ReactiveCommandExtended.Create<string>(Search, exceptionContainer);
    }

    public ObservableCollection<Recipe> Items { get; private set; }

    public ReactiveCommand<Recipe, Unit> ShowRecipeCommand { get; }
    public ReactiveCommand<string, Unit> SearchCommand { get; }

    private ViewModelBase ShowRecipe(Recipe recipe, RecipeViewFactory factory)
    {
        return factory.Create(recipe, this);
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