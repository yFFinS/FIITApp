using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Presentation.Interfaces;

namespace Recipes.Presentation.ViewModels;

public class RecipeListViewModel : ViewModelBase
{
    public IImageLoader ImageLoader { get; }
    private List<Recipe> Recipes { get; }
    
    public ReactiveCommand<Recipe, Unit> ShowRecipeCommand { get; }

    public RecipeListViewModel(List<Recipe> recipes, Lazy<IViewContainer> container, IImageLoader imageLoader)
    {
        Recipes = recipes;
        ImageLoader = imageLoader;
        ShowRecipeCommand = ReactiveCommand.Create<Recipe>(recipe => ShowRecipe(recipe, container, imageLoader));
    }
    
    private void ShowRecipe(Recipe recipe, Lazy<IViewContainer> container, IImageLoader loader)
    {
        container.Value.Content = new RecipeViewModel(recipe, container, loader, this);
    }
}