using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Presentation.Controls;

public partial class RecipeCard : UserControl
{
    public static readonly DirectProperty<RecipeCard, IImageLoader> ImageLoaderProperty =
        AvaloniaProperty.RegisterDirect<RecipeCard, IImageLoader>(
            nameof(ImageLoader),
            o => o.ImageLoader,
            (o, v) => o.ImageLoader = v);

    private IImageLoader _imageLoader;
    
    public IImageLoader ImageLoader
    {
        get => _imageLoader;
        set => SetAndRaise(ImageLoaderProperty, ref _imageLoader, value);
    }
    
    public static readonly DirectProperty<RecipeCard, Recipe> RecipeProperty =
        AvaloniaProperty.RegisterDirect<RecipeCard, Recipe>(
            nameof(ImageLoader),
            o => o.Recipe,
            (o, v) => o.Recipe = v);

    private Recipe _recipe;
    
    public Recipe Recipe
    {
        get => _recipe;
        set => SetAndRaise(RecipeProperty, ref _recipe, value);
    }

    public Task<Bitmap> Image => ImageLoader.LoadImage(Recipe.ImageUrl);

    public RecipeCard()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}