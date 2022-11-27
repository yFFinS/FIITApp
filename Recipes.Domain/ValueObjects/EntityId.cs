using System.Xml.Serialization;
using Ardalis.GuardClauses;
using Recipes.Domain.Base;

namespace Recipes.Domain.ValueObjects;

public class EntityId : ValueObject<EntityId>
{
    [XmlAttribute("value")] public Guid Value { get; }

    public EntityId(string value) : this(Guid.Parse(value))
    {
    }

    public EntityId(Guid value)
    {
        Value = Guard.Against.Default(value);
    }

    public override string ToString() => Value.ToString();


    public static EntityId NewId() => new(Guid.NewGuid());

    private EntityId()
    {
    }
}