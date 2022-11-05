using System;

namespace Recipes.Presentation.ViewModels;

public class MainViewItem : ViewModelBase
{
    public readonly Action<ViewModelBase> SetMainViewContent;
    
    public MainViewItem(Action<ViewModelBase> setContent)
    {
        SetMainViewContent = setContent;
    }
}