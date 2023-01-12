using Recipes.Presentation.ViewModels;
using System;

namespace Recipes.Presentation.DataTypes;

public class MainMenuItem
{
    public string Title { get; set; }
    public Func<ViewModelBase> Page { get; set; }
}