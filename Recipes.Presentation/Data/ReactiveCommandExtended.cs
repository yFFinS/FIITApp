using ReactiveUI;
using Recipes.Presentation.Interfaces;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace Recipes.Presentation.DataTypes;

public static class ReactiveCommandExtended
{
    public static ReactiveCommand<TParam, Unit> Create<TParam>(Action<TParam> execute,
        IExceptionContainer container)
    {
        var command = ReactiveCommand.Create(execute);
        command.ThrownExceptions.Subscribe(container.AddException);
        return command;
    }

    public static ReactiveCommand<Unit, Unit> Create(Action execute,
        IExceptionContainer container)
    {
        var command = ReactiveCommand.Create(execute);
        command.ThrownExceptions.Subscribe(container.AddException);
        return command;
    }

    public static ReactiveCommand<TParam, TResult> CreateFromTask<TParam, TResult>(Func<TParam, Task<TResult>> execute,
        IExceptionContainer container)
    {
        var command = ReactiveCommand.CreateFromTask(execute);
        command.ThrownExceptions.Subscribe(container.AddException);
        return command;
    }
}