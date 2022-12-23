using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Base;

public abstract class Entity<TId>
{
    private TId _id;

    public TId Id
    {
        get => _id;
        set {}
    }

    protected Entity(TId id)
    {
        _id = id;
    }

    protected Entity() { }
}