using System;
using Avalonia.Media.Imaging;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;
using System.Threading.Tasks;

namespace Recipes.Presentation.DataTypes;

public class ImageWrapper<T>
{
    public ImageWrapper(T o, IImageLoader imageLoader, Uri? imageUrl)
    {
        Object = o;
        ImageLoader = imageLoader;
        ImageUrl = imageUrl;
    }

    public T Object { get; set; }
    public IImageLoader ImageLoader { get; }
    public Uri? ImageUrl { get; }
    public Task<Bitmap> Image => ImageLoader.LoadImage(ImageUrl);

    public static implicit operator T(ImageWrapper<T> wrapper) => wrapper.Object;
}