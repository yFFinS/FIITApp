using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Presentation.ViewModels;

public class RecipeViewModel : ViewModelBase
{
    public Recipe Recipe { get; set; }
    
    public RecipeViewModel(Recipe recipe)
    {
        Recipe = recipe;
    }
}