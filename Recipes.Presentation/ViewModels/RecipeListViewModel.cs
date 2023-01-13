using Avalonia.Controls;
using ReactiveUI;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Presentation.DataTypes;
using Recipes.Presentation.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;

namespace Recipes.Presentation.ViewModels;

public class RecipeListViewModel : ViewModelBase
{
#if DEBUG
    public RecipeListViewModel() { }
#endif
    public IImageLoader ImageLoader { get; }
    private List<ImageWrapper<Recipe>> Recipes { get; }

    public ReactiveCommand<Recipe, Unit> ShowRecipeCommand { get; }

    public RecipeListViewModel(List<Recipe> recipes, IViewContainer container, IImageLoader imageLoader,
        RecipeViewFactory factory, IExceptionContainer exceptionContainer)
    {
        Recipes = recipes.Select(i => new ImageWrapper<Recipe>(i, imageLoader)).ToList();
        ImageLoader = imageLoader;
        ShowRecipeCommand =
            ReactiveCommandExtended.Create<Recipe>(recipe => ShowRecipe(recipe, container, factory),
                exceptionContainer);
    }

    private void ShowRecipe(Recipe recipe, IViewContainer container, RecipeViewFactory factory)
    {
        container.Content = factory.Create(recipe, this);
    }
}