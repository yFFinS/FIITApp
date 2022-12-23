using Recipes.Domain.ValueObjects;
using System.Collections;

namespace Recipes.Domain.IngredientsAggregate;

public class IngredientGroup : IEnumerable<Ingredient>
{
    private readonly Dictionary<EntityId, Ingredient> _ingredients;

    public int Count => Ingredients.Count;

    public Dictionary<EntityId, Ingredient> Ingredients
    {
        get => _ingredients;
        set => throw new NotSupportedException();
    }

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

        Ingredients.Add(ingredient.ProductId, ingredient);
    }

    public void Update(Ingredient ingredient)
    {
        ThrowIfIngredientDoesNotExist(ingredient.ProductId);

        Ingredients[ingredient.ProductId] = ingredient;
    }

    public void RemoveByProductId(EntityId productId)
    {
        ThrowIfIngredientDoesNotExist(productId);

        Ingredients.Remove(productId);
    }

    public void Remove(Ingredient ingredient) => RemoveByProductId(ingredient.ProductId);

    public Ingredient GetByProductId(EntityId productId)
    {
        ThrowIfIngredientDoesNotExist(productId);

        return Ingredients[productId];
    }

    public Ingredient? TryGetByProductId(EntityId productId)
    {
        return Ingredients.TryGetValue(productId, out var ingredient) ? ingredient : null;
    }

    public IEnumerator<Ingredient> GetEnumerator()
    {
        return Ingredients.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IReadOnlyCollection<Ingredient> AsReadOnlyCollection() => Ingredients.Values.ToList();

    private void ThrowIfIngredientDoesNotExist(EntityId productId)
    {
        if (!Ingredients.ContainsKey(productId))
        {
            throw new IngredientNotFoundException(productId);
        }
    }

    private void ThrowIfIngredientExists(EntityId productId)
    {
        if (Ingredients.ContainsKey(productId))
        {
            throw new IngredientExistsException(productId);
        }
    }
}