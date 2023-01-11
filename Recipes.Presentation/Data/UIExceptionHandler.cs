using System;
using System.Reactive;

namespace Recipes.Presentation.DataTypes;

public class UiExceptionHandler : IObserver<Exception>
{
    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(Exception value)
    {
    }
}