using Avalonia.Media.Imaging;

namespace Recipes.Application.Interfaces;

public interface IImageLoader
{
    Task<Bitmap> LoadImage(Uri imageUri);
}