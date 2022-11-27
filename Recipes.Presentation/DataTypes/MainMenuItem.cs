using Recipes.Presentation.ViewModels;

namespace Recipes.Presentation.DataTypes;

public class MainMenuItem
{
    public string Title { get; set; }
    public ViewModelBase Page { get; set; }
}