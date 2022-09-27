using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Presentation.ViewModels;

public class SearchViewModel : ViewModelBase
{
    public SearchViewModel(IEnumerable<string> items)
    {
        Items = new ObservableCollection<string>(items);
    }

    public ObservableCollection<string> Items { get; }
}

internal class RecipesDataBase
{
    public IEnumerable<string> Items => Enumerable.Repeat<string>("Apple", 100);
}