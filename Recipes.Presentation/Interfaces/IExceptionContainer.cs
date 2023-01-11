using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Recipes.Presentation.Interfaces;

public interface IExceptionContainer
{
    public ObservableCollection<Exception> Errors { get; }
    public bool HasErrors { get; }
    public void AddException(Exception exception);
}