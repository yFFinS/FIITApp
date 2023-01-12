using Recipes.Domain.Base;
using System.Xml.Serialization;

namespace Recipes.Infrastructure;

[XmlType(nameof(Entity<T>))]
public class EntityDbo<T> : IEquatable<T> where T : EntityDbo<T>
{
    public string Id { get; set; }

    public bool Equals(T? other)
    {
        return other is not null && Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((EntityDbo<T>)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(EntityDbo<T>? left, EntityDbo<T>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(EntityDbo<T>? left, EntityDbo<T>? right)
    {
        return !Equals(left, right);
    }
}