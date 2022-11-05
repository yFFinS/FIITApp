using Recipes.Presentation.ViewModels;

namespace Recipes.Presentation.Interfaces;

public interface IViewContainer
{
    public ViewModelBase Content { get; set; }
}