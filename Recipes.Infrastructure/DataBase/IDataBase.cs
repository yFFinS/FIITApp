using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Infrastructure;

public interface IDataBase
{
    public void InsertProduct(Product obj);
    public List<Product> GetAllProducts();
    public void InsertRecipe(RecipeDataBaseObject obj);
    public List<RecipeDataBaseObject> GetAllRecipes();
}