using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Recipes.Presentation.Views;

public partial class RecipeSearchView : UserControl
{
    public RecipeSearchView()
    {
        InitializeComponent();
        // DataContext = new SearchViewModel(new RecipesDataBase().Items);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}