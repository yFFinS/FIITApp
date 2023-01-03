using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.Services;
using Recipes.Domain.ValueObjects;

namespace Recipes.DatabaseEditor;

public class RecipeParsingException : Exception
{
    protected RecipeParsingException(string message) : base(message)
    {
    }
}

public class QuantityParsingException : RecipeParsingException
{
    public QuantityParsingException(string quantity) : base($"Could not parse quantity: {quantity}")
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
    private readonly ILogger<RecipeParser> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IQuantityParser _quantityParser;

    public RecipeParser(ILogger<RecipeParser> logger, IProductRepository productRepository,
        IQuantityParser quantityParser)
    {
        _logger = logger;
        _productRepository = productRepository;
        _quantityParser = quantityParser;
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
        var split = line.Split(" - ");
        var name = split[0].Trim();
        var quantityString = split[1].Trim();

        var quantity = _quantityParser.TryParseQuantity(quantityString);
        if (quantity is null)
        {
            throw new QuantityParsingException(quantityString);
        }

        var product = _productRepository.GetProductByNameAsync(name).Result;
        if (product is null)
        {
            throw new ProductMissingException(name);
        }

        return new Ingredient(product, quantity);
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


        int servings;
        string? imageUrl;
        string? description;
        TimeSpan cookingTime;

        try
        {
            imageUrl = ParseValue(lines[1]);
            imageUrl = string.IsNullOrWhiteSpace(imageUrl) ? null : imageUrl;

            servings = int.Parse(ParseValue(lines[2]));
            cookingTime = TimeSpan.FromSeconds(int.Parse(ParseValue(lines[3])));
            description = ParseValue(lines[4]);
            description = string.IsNullOrWhiteSpace(description) ? null : description;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to parse recipe");
            return null;
        }

        var ingredients = new List<Ingredient>();

        var ingredientsIndex = 6;
        while (IsIngredient(lines[ingredientsIndex]))
        {
            ingredients.Add(ParseIngredient(lines[ingredientsIndex]));
            ingredientsIndex++;
        }

        var cookingTechniqueStartIndex = ingredientsIndex + 1;
        var cookingSteps = lines.Skip(cookingTechniqueStartIndex)
            .Select(ParseCookingStep)
            .ToList();

        return new Recipe(EntityId.NewId(), title, description, servings, cookingTime,
            new IngredientGroup(ingredients), new CookingTechnic(cookingSteps))
        {
            ImageUrl = imageUrl is null ? null : new Uri(imageUrl)
        };
    }
}