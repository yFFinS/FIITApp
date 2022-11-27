using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Recipes.Presentation.Views;

public partial class RecipeListView : UserControl
{
    public RecipeListView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}