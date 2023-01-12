using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Recipes.Presentation.Views;

public partial class RecipeEditorView : UserControl
{
    public RecipeEditorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}