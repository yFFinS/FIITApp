using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ReactiveUI;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Presentation.Interfaces;

namespace Recipes.Presentation.ViewModels;

public class RecipeViewModel : ViewModelBase
{
#if DEBUG
    public RecipeViewModel() { }
#endif

    public Recipe Recipe { get; set; }
    public IProductRepository Repository { get; }
    public IImageLoader ImageLoader { get; }
    public ReactiveCommand<Unit, Unit> BackCommand { get; }


    public RecipeViewModel(Recipe recipe, Lazy<IViewContainer> container, IImageLoader imageLoader, IProductRepository repository, ViewModelBase parent)
    {
        Recipe = recipe;
        ImageLoader = imageLoader;
        Repository = repository;
        BackCommand = ReactiveCommand.Create(() =>
        {
            container.Value.Content = parent;
        });
    }

    public Task<Bitmap> Image => ImageLoader.LoadImage(Recipe.ImageUrl);
}