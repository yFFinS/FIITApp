using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;
using System.Threading.Tasks;

namespace Recipes.Presentation.Controls;

public partial class RecipeCard : UserControl
{
    public RecipeCard()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}