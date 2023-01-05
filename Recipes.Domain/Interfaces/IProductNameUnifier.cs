namespace Recipes.Domain.Interfaces;

public interface IProductNameUnifier
{
    string GetUnifiedName(string productName);
}