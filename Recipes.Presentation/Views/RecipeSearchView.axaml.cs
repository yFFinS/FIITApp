using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Recipes.Presentation.ViewModels;

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