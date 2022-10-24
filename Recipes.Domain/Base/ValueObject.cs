using System.Linq.Expressions;
using System.Reflection;

namespace Recipes.Domain.Base;

public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
{
    private static readonly Func<T, T, bool> EqualsExpression;
    private static readonly Func<T, int> GetHashCodeExpression;

    static ValueObject()
    {
        var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.GetCustomAttribute<EqualityIgnoreAttribute>() is null)
            .ToArray();

        var left = Expression.Parameter(typeof(T), "left");
        var right = Expression.Parameter(typeof(T), "right");
        var body = properties
            .Select(p => Expression.Equal(Expression.Property(left, p), Expression.Property(right, p)))
            .Aggregate(Expression.AndAlso);

        EqualsExpression = Expression.Lambda<Func<T, T, bool>>(body, left, right).Compile();

        var me = Expression.Parameter(typeof(T), "me");
        var hash = properties
            .Select(p => Expression.Property(me, p))
            // TODO: doesn't work for nullables
            // .Select(p => Expression.Condition(
            //     Expression.Equal(p, Expression.Constant(null)),
            //     Expression.Constant(0),
            //     Expression.Call(p, "GetHashCode", Type.EmptyTypes)))
            .Select(p => Expression.Call(p, "GetHashCode", Type.EmptyTypes))
            .Select(p => (Expression)p)
            .Aggregate(Expression.ExclusiveOr);
        GetHashCodeExpression = Expression.Lambda<Func<T, int>>(hash, me).Compile();
    }

    public virtual bool Equals(T? other)
    {
        if (other is null || GetType() != other.GetType())
        {
            return false;
        }

        return ReferenceEquals(this, other) || EqualsExpression((T)this, other);
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueObject<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return GetHashCodeExpression((T)this);
    }

    public static bool operator ==(ValueObject<T>? left, ValueObject<T>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ValueObject<T>? left, ValueObject<T>? right)
    {
        return !Equals(left, right);
    }
}