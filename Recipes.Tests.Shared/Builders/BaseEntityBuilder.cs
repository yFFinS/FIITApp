using Recipes.Domain.Base;
using Recipes.Domain.ValueObjects;

namespace Recipes.Tests.Shared.Builders;

public abstract class BaseEntityBuilder<TEntity, TBuilder> : AbstractBuilder<TEntity>
    where TEntity : BaseEntity
    where TBuilder : BaseEntityBuilder<TEntity, TBuilder>
{
    protected EntityId Id { get; set; } = EntityId.NewId();

    public TBuilder WithId(EntityId id)
    {
        Id = id;
        return (TBuilder)this;
    }

    protected override TEntity Build(object?[] arguments)
    {
        var argumentsWithId = new List<object?> { Id }.Concat(arguments).ToArray();
        return base.Build(argumentsWithId);
    }
}