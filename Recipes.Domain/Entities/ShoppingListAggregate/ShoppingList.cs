using Recipes.Domain.Base;
using Recipes.Domain.Entities.IngredientAggregate;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.ShoppingListAggregate;

public class ShoppingList : BaseEntity
{
    private readonly Dictionary<EntityId, Recipe> _recipes;
    private readonly Ingredients _ingredients;

    public ShoppingList(EntityId id) : base(id)
    {
        _recipes = new Dictionary<EntityId, Recipe>();
        _ingredients = new Ingredients();
    }

    public ShoppingList(EntityId id, IEnumerable<Recipe> recipes, Ingredients ingredients) : base(id)
    {
        ArgumentNullException.ThrowIfNull(recipes);
        ArgumentNullException.ThrowIfNull(ingredients);

        _recipes = recipes.ToDictionary(r => r.Id);
        _ingredients = ingredients;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        _ingredients.Add(ingredient);
    }

    public void RemoveIngredient(EntityId ingredientId)
    {
        _ingredients.Remove(ingredientId);
    }

    public void UpdateIngredient(Ingredient ingredient)
    {
        _ingredients.Update(ingredient);
    }

    public void AddRecipe(Recipe recipe)
    {
        ArgumentNullException.ThrowIfNull(recipe);

        ThrowIfRecipeAlreadyExists(recipe.Id);
        _recipes.Add(recipe.Id, recipe);
    }

    public void RemoveRecipe(EntityId recipeId)
    {
        ThrowIfRecipeDoesNotExist(recipeId);
        _recipes.Remove(recipeId);
    }

    public void UpdateRecipe(Recipe recipe)
    {
        ArgumentNullException.ThrowIfNull(recipe);

        ThrowIfRecipeDoesNotExist(recipe.Id);
        _recipes[recipe.Id] = recipe;
    }

    private void ThrowIfRecipeDoesNotExist(EntityId recipeId)
    {
        if (!_recipes.ContainsKey(recipeId))
        {
            throw new RecipeNonexistentException(recipeId);
        }
    }

    private void ThrowIfRecipeAlreadyExists(EntityId recipeId)
    {
        if (_recipes.ContainsKey(recipeId))
        {
            throw new RecipeExistsException(recipeId);
        }
    }

    public IReadOnlyCollection<Recipe> Recipes => _recipes.Values.ToList();
    public IReadOnlyCollection<Ingredient> Ingredients => _ingredients.AsReadOnlyCollection();
}