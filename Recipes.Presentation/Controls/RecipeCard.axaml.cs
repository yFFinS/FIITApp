using Avalonia.Controls;
using Avalonia.Markup.Xaml;

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