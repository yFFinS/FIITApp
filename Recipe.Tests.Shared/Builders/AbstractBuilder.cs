using System.Reflection;

namespace Recipes.Tests.Shared.Builders;

public abstract class AbstractBuilder<TResult>
{
    protected virtual TResult Build(IList<object?> arguments)
    {
        try
        {
            return (TResult)Activator.CreateInstance(typeof(TResult), arguments)!;
        }
        catch (TargetInvocationException e)
        {
            // Rethrow the inner exception for testing purposes
            throw e.InnerException!;
        }
    }

    protected abstract IEnumerable<object?> GetConstructorArguments();

    public TResult Build()
    {
        var arguments = GetConstructorArguments().ToList();
        return Build(arguments);
    }

    public static implicit operator TResult(AbstractBuilder<TResult> builder)
    {
        return builder.Build();
    }
}