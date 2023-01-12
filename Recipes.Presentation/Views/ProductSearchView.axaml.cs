using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Recipes.Presentation.Views;

public partial class ProductSearchView : UserControl
{
    public ProductSearchView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}