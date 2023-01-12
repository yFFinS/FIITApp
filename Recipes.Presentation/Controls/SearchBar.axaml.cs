using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Recipes.Presentation.Controls;

public partial class SearchBar : UserControl
{
    public SearchBar()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}