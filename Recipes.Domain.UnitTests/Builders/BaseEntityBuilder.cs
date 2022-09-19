using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.UnitTests.Builders;

public abstract class BaseEntityBuilder<TEntity, TBuilder>
    where TEntity : BaseEntity
    where TBuilder : BaseEntityBuilder<TEntity, TBuilder>
{
    protected EntityId Id { get; set; } = EntityId.NewId();

    protected abstract IEnumerable<object?> GetConstructorArguments();

    public TBuilder WithId(EntityId id)
    {
        Id = id;
        return (TBuilder)this;
    }

    public TEntity Build()
    {
        var arguments = new List<object> { Id }.Concat(GetConstructorArguments());
        try
        {
            return (TEntity)Activator.CreateInstance(typeof(TEntity), arguments.ToArray())!;
        }
        catch (TargetInvocationException e)
        {
            // Rethrow the inner exception for testing purposes
            throw e.InnerException!;
        }
    }

    public static implicit operator TEntity(BaseEntityBuilder<TEntity, TBuilder> builder)
    {
        return builder.Build();
    }
}