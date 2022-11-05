using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Recipes.Presentation.Views;

public partial class SelectionView : UserControl
{
    public SelectionView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}