namespace Recipes.Domain.Base;

[AttributeUsage(AttributeTargets.Property)]
public sealed class EqualityIgnoreAttribute : Attribute
{
}