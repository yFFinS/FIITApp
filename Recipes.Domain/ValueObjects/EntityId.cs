using Ardalis.GuardClauses;
using Recipes.Domain.Base;

namespace Recipes.Domain.ValueObjects;

public class EntityId : ValueObject
{
    public readonly Guid Value;

    public EntityId(string value) : this(Guid.Parse(value))
    {
    }

    public EntityId(Guid value)
    {
        Value = Guard.Against.Default(value);
    }

    public override string ToString() => Value.ToString();


    public static EntityId New() => new(Guid.NewGuid());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}