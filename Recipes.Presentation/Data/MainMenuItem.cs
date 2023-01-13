using Recipes.Presentation.ViewModels;
using System;

namespace Recipes.Presentation.DataTypes;

public class MainMenuItem
{
    public string Title { get; set; }
    public Func<ViewModelBase> PageFactory { get; set; }
}

internal class ProductSearchMenuItem : MainMenuItem
{
    public ProductSearchMenuItem(Func<ProductSearchViewModel> pageFactory)
    {
        PageFactory = pageFactory;
        Title = "Искать по ингредиентам";
    }
}

internal class RecipeSearchMenuItem : MainMenuItem
{
    public RecipeSearchMenuItem(Func<RecipeSearchViewModel> pageFactory)
    {
        PageFactory = pageFactory;
        Title = "Искать по названию";
    }
}

internal class RecipeEditorMenuItem : MainMenuItem
{
    public RecipeEditorMenuItem(Func<RecipeEditorViewModel> pageFactory)
    {
        PageFactory = pageFactory;
        Title = "Добавить свой рецепт";
    }
}