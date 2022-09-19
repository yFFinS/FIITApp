using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Base;

public abstract class BaseEntity
{
    public EntityId Id { get; }

    protected BaseEntity(EntityId id)
    {
        Id = id;
    }
}