using Recipes.Domain.Base;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.ShoppingListAggregate;

public class ShoppingList : BaseEntity
{
    private readonly Dictionary<EntityId, Recipe> _recipes;
    private readonly IngredientGroup _ingredientGroup;

    public ShoppingList(EntityId id) : base(id)
    {
        _recipes = new Dictionary<EntityId, Recipe>();
        _ingredientGroup = new IngredientGroup();
    }

    public ShoppingList(EntityId id, IEnumerable<Recipe> recipes, IngredientGroup ingredientGroup) : base(id)
    {
        ArgumentNullException.ThrowIfNull(recipes);
        ArgumentNullException.ThrowIfNull(ingredientGroup);

        _recipes = recipes.ToDictionary(r => r.Id);
        _ingredientGroup = ingredientGroup;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        _ingredientGroup.Add(ingredient);
    }

    public void RemoveIngredient(EntityId ingredientId)
    {
        _ingredientGroup.RemoveByProductId(ingredientId);
    }

    public void UpdateIngredient(Ingredient ingredient)
    {
        _ingredientGroup.Update(ingredient);
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
    public IReadOnlyCollection<Ingredient> Ingredients => _ingredientGroup.AsReadOnlyCollection();
}