using Recipes.Domain.ValueObjects;

namespace Recipes.Infrastructure;

public interface IDataBase
{
    public void InsertProduct(ProductDbo product);
    public List<ProductDbo> GetAllProducts();
    public void InsertRecipe(RecipeDbo recipe, bool useUserDatabase);
    public List<RecipeDbo> GetAllRecipes(bool onlyGlobal = false);
    void DeleteProduct(EntityId product);
    void DeleteRecipe(EntityId recipe);
}