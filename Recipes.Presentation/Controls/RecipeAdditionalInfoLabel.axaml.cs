﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Recipes.Presentation.Controls;

public partial class RecipeAdditionalInfoLabel : UserControl
{
    public RecipeAdditionalInfoLabel()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}