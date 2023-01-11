using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Presentation.Interfaces;
using Recipes.Presentation.ViewModels;

namespace Recipes.Presentation.DataTypes;

public class RecipeViewFactory
{
    private readonly IViewContainer _container;
    private readonly IImageLoader _imageLoader;
    private readonly IProductRepository _productRepository;

    public RecipeViewFactory(IViewContainer container, IImageLoader imageLoader, IProductRepository productRepository)
    {
        _container = container;
        _imageLoader = imageLoader;
        _productRepository = productRepository;
    }

    public RecipeViewModel Create(Recipe recipe, ViewModelBase parent)
    {
        return new RecipeViewModel(recipe, _container, _imageLoader, _productRepository, parent);
    }
}