using System.Collections.Generic;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Presentation.ViewModels;

internal class RecipeEditorViewModel : ViewModelBase
{
    public string Title { get; set; }

    public List<Ingredient> Ingredients { get; set; }

    public string Description { get; set; }

    public object Servings { get; set; }

    public RecipeEditorViewModel()
    {
    }
}