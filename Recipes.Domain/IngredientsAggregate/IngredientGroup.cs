using Recipes.Domain.ValueObjects;
using System.Collections;

namespace Recipes.Domain.IngredientsAggregate;

public class IngredientGroup : IEnumerable<Ingredient>
{
    private readonly Dictionary<EntityId, Ingredient> _ingredients;

    public int Count => _ingredients.Count;

    public IngredientGroup()
    {
        _ingredients = new Dictionary<EntityId, Ingredient>();
    }

    public IngredientGroup(IEnumerable<Ingredient> ingredients)
    {
        _ingredients = ingredients.ToDictionary(ingredient => ingredient.ProductId);
    }

    public void Add(Ingredient ingredient)
    {
        ThrowIfIngredientExists(ingredient.ProductId);

        _ingredients.Add(ingredient.ProductId, ingredient);
    }

    public void Update(Ingredient ingredient)
    {
        ThrowIfIngredientDoesNotExist(ingredient.ProductId);

        _ingredients[ingredient.ProductId] = ingredient;
    }

    public void RemoveByProductId(EntityId productId)
    {
        ThrowIfIngredientDoesNotExist(productId);

        _ingredients.Remove(productId);
    }

    public void Remove(Ingredient ingredient) => RemoveByProductId(ingredient.ProductId);

    public Ingredient GetByProductId(EntityId productId)
    {
        ThrowIfIngredientDoesNotExist(productId);

        return _ingredients[productId];
    }

    public Ingredient? TryGetByProductId(EntityId productId)
    {
        return _ingredients.TryGetValue(productId, out var ingredient) ? ingredient : null;
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

    private void ThrowIfIngredientDoesNotExist(EntityId productId)
    {
        if (!_ingredients.ContainsKey(productId))
        {
            throw new IngredientNotFoundException(productId);
        }
    }

    private void ThrowIfIngredientExists(EntityId productId)
    {
        if (_ingredients.ContainsKey(productId))
        {
            throw new IngredientExistsException(productId);
        }
    }
}