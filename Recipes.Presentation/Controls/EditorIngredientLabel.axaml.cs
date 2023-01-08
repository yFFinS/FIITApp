using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Recipes.Presentation.Controls;

public partial class EditorIngredientLabel : UserControl
{
    public EditorIngredientLabel()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}