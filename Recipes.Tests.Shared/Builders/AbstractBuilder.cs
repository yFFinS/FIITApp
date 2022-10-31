using System.Reflection;

namespace Recipes.Tests.Shared.Builders;

public abstract class AbstractBuilder<TResult>
{
    public abstract TResult Build();

    public static implicit operator TResult(AbstractBuilder<TResult> builder)
    {
        return builder.Build();
    }
}