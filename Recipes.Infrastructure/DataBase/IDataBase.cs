using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Infrastructure;

public interface IDataBase
{
    public void InsertProduct(ProductDbo product);
    public List<ProductDbo> GetAllProducts();
    public void InsertRecipe(RecipeDbo obj);
    public List<RecipeDbo> GetAllRecipes();
}