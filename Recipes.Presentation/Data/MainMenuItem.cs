using System;
using Recipes.Presentation.ViewModels;

namespace Recipes.Presentation.DataTypes;

public class MainMenuItem
{
    public string Title { get; set; }
    public Func<ViewModelBase> Page { get; set; }
}