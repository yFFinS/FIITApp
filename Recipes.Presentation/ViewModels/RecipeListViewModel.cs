using Avalonia.Controls;
using ReactiveUI;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Presentation.DataTypes;
using Recipes.Presentation.Interfaces;
using System.Collections.Generic;
using System.Reactive;

namespace Recipes.Presentation.ViewModels;

public class RecipeListViewModel : ViewModelBase
{
#if DEBUG
    public RecipeListViewModel() { }
#endif
    public IImageLoader ImageLoader { get; }
    private List<Recipe> Recipes { get; }

    public ReactiveCommand<Recipe, Unit> ShowRecipeCommand { get; }
    public ReactiveCommand<ScrollViewer, Unit> NextPageCommand { get; }
    public ReactiveCommand<ScrollViewer, Unit> PreviousPageCommand { get; }

    public RecipeListViewModel(List<Recipe> recipes, IViewContainer container, IImageLoader imageLoader,
        RecipeViewFactory factory, IExceptionContainer exceptionContainer)
    {
        Recipes = recipes;
        ImageLoader = imageLoader;
        ShowRecipeCommand =
            ReactiveCommandExtended.Create<Recipe>(recipe => ShowRecipe(recipe, container, factory),
                exceptionContainer);
        PreviousPageCommand = ReactiveCommandExtended.Create<ScrollViewer>(viewer => viewer.PageLeft(), exceptionContainer);
        NextPageCommand = ReactiveCommandExtended.Create<ScrollViewer>(viewer => viewer.PageRight(), exceptionContainer);
    }

    private void ShowRecipe(Recipe recipe, IViewContainer container, RecipeViewFactory factory)
    {
        container.Content = factory.Create(recipe, this);
    }
}