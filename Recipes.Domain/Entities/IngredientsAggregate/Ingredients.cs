using System.Collections;

namespace Recipes.Domain.Entities.IngredientsAggregate;

public class Ingredients : IEnumerable<Ingredient>
{
    private readonly Dictionary<string, Ingredient> _ingredients;

    public Ingredients()
    {
        _ingredients = new Dictionary<string, Ingredient>();
    }

    public Ingredients(IEnumerable<Ingredient> ingredients)
    {
        _ingredients = ingredients.ToDictionary(x => x.Name);
    }

    public void AddIngredient(Ingredient ingredient)
    {
        ThrowIfIngredientExists(ingredient.Name);

        _ingredients.Add(ingredient.Name, ingredient);
    }

    public void UpdateIngredient(string ingredientName, int quantity)
    {
        ThrowIfIngredientDoesNotExist(ingredientName);

        // TODO: Implement
        // _ingredients[ingredientName].UpdateQuantity(quantity);
    }

    public void RemoveIngredient(string ingredientName)
    {
        if (!_ingredients.ContainsKey(ingredientName))
        {
            throw new IngredientNotFoundException(ingredientName);
        }

        _ingredients.Remove(ingredientName);
    }

    public Ingredient GetIngredient(string ingredientName)
    {
        ThrowIfIngredientDoesNotExist(ingredientName);

        return _ingredients[ingredientName];
    }

    public Ingredient? TryGetIngredient(string ingredientName)
    {
        return _ingredients.TryGetValue(ingredientName, out var ingredient) ? ingredient : null;
    }

    public IEnumerator<Ingredient> GetEnumerator()
    {
        return _ingredients.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void ThrowIfIngredientDoesNotExist(string ingredientName)
    {
        if (!_ingredients.ContainsKey(ingredientName))
        {
            throw new IngredientNotFoundException(ingredientName);
        }
    }

    private void ThrowIfIngredientExists(string ingredientName)
    {
        if (_ingredients.ContainsKey(ingredientName))
        {
            throw new IngredientExistsException(ingredientName);
        }
    }
}