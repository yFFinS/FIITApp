using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Recipes.Domain.IngredientsAggregate;

namespace Recipes.Presentation.Controls;

public partial class IngredientLabel : UserControl
{
    public static readonly DirectProperty<IngredientLabel, Ingredient> IngredientProperty =
        AvaloniaProperty.RegisterDirect<IngredientLabel, Ingredient>(
            nameof(Ingredient),
            o => o.Ingredient,
            (o, v) => o.Ingredient = v);

    private Ingredient _ingredient;

    public Ingredient Ingredient
    {
        get => _ingredient;
        set => SetAndRaise(IngredientProperty, ref _ingredient, value);
    }

    public IngredientLabel()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}