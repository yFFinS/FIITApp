using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.DatabaseEditor;

public class RecipeParsingException : Exception
{
    public RecipeParsingException(string message) : base(message)
    {
    }
}

public class IngredientsMissingException : Exception
{
    public IngredientsMissingException() : base("Ingredients are missing")
    {
    }
}

public class CookingTechniqueMissingException : Exception
{
    public CookingTechniqueMissingException() : base("Cooking technique is missing")
    {
    }
}

public class ProductMissingException : RecipeParsingException
{
    public ProductMissingException(string productName) : base($"Product {productName} is missing")
    {
    }
}

public class RecipeParser
{
    private readonly IProductRepository _productRepository;

    public RecipeParser(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    private static string ParseValue(string line)
    {
        var index = line.IndexOf(':');
        return line[(index + 1)..].Trim();
    }

    private static bool IsIngredient(string line)
    {
        return line.Contains('-');
    }

    private Ingredient ParseIngredient(string line)
    {
        var index = line.IndexOf('-');
        var name = line[..index].Trim();
        var amount = line[(index + 1)..].Trim();

        var quantity = Quantity.TryParse(amount);
        if (quantity is null)
        {
            throw new Exception($"Invalid quantity: {amount}");
        }

        var product = _productRepository.GetProductByNameAsync(name).Result;
        if (product is null)
        {
            throw new ProductMissingException(name);
        }

        return new Ingredient(product.Id, quantity);
    }

    private static CookingStep ParseCookingStep(string line)
    {
        var index = line.IndexOf('.');
        var description = line[(index + 1)..].Trim();

        return new CookingStep(description);
    }

    public Recipe? TryParseRecipe(string text)
    {
        var lines = text.Split(Environment.NewLine);
        var title = lines[0];
        var description = (string?)null;

        var servings = int.Parse(ParseValue(lines[1]));
        var cookingTime = TimeSpan.Parse(ParseValue(lines[2]));
        var calories = int.Parse(ParseValue(lines[3]));
        var proteins = int.Parse(ParseValue(lines[4]));
        var fats = int.Parse(ParseValue(lines[5]));
        var carbohydrates = int.Parse(ParseValue(lines[6]));

        var energyValue = new EnergyValue(calories, proteins, fats, carbohydrates);

        var ingredientsStartIndex = lines.Select((line, idx) => (line, idx))
            .First(x => x.line == "Ингредиенты:").idx + 1;

        var ingredients = new List<Ingredient>();

        while (IsIngredient(lines[ingredientsStartIndex]))
        {
            ingredients.Add(ParseIngredient(lines[ingredientsStartIndex]));
            ingredientsStartIndex++;
        }

        if (ingredients.Count == 0)
        {
            throw new IngredientsMissingException();
        }

        var cookingTechniqueStartIndex = ingredientsStartIndex + 1;
        var cookingSteps = lines.Skip(cookingTechniqueStartIndex)
            .Select(ParseCookingStep)
            .ToList();

        if (cookingSteps.Count == 0)
        {
            throw new CookingTechniqueMissingException();
        }

        return new Recipe(EntityId.NewId(), title, description, servings, cookingTime, energyValue,
            new IngredientGroup(ingredients), new CookingTechnic(cookingSteps));
    }
}