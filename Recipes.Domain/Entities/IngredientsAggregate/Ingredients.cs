using System.Collections;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Entities.IngredientAggregate;

public class Ingredients : IEnumerable<Ingredient>
{
    private readonly Dictionary<EntityId, Ingredient> _ingredients;

    public Ingredients()
    {
        _ingredients = new Dictionary<EntityId, Ingredient>();
    }

    public Ingredients(IEnumerable<Ingredient> ingredients)
    {
        _ingredients = ingredients.ToDictionary(x => x.Id);
    }

    public void Add(Ingredient ingredient)
    {
        ThrowIfIngredientExists(ingredient.Id);

        _ingredients.Add(ingredient.Id, ingredient);
    }

    public void Update(Ingredient ingredient)
    {
        ThrowIfIngredientDoesNotExist(ingredient.Id);

        _ingredients[ingredient.Id] = ingredient;
    }

    public void Remove(EntityId ingredientId)
    {
        ThrowIfIngredientDoesNotExist(ingredientId);

        _ingredients.Remove(ingredientId);
    }

    public Ingredient GetIngredientById(EntityId ingredientId)
    {
        ThrowIfIngredientDoesNotExist(ingredientId);

        return _ingredients[ingredientId];
    }

    public Ingredient? TryGetIngredientById(EntityId ingredientId)
    {
        return _ingredients.TryGetValue(ingredientId, out var ingredient) ? ingredient : null;
    }

    public IEnumerator<Ingredient> GetEnumerator()
    {
        return _ingredients.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IReadOnlyCollection<Ingredient> AsReadOnlyCollection() => _ingredients.Values.ToList();

    private void ThrowIfIngredientDoesNotExist(EntityId ingredientId)
    {
        if (!_ingredients.ContainsKey(ingredientId))
        {
            throw new IngredientNotFoundException(ingredientId);
        }
    }

    private void ThrowIfIngredientExists(EntityId ingredientId)
    {
        if (_ingredients.ContainsKey(ingredientId))
        {
            throw new IngredientExistsException(ingredientId);
        }
    }
}