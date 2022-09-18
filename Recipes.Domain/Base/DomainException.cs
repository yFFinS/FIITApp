namespace Recipes.Domain.Base;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}