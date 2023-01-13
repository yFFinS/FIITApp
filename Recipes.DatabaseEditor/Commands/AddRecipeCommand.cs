using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Interfaces;

namespace Recipes.DatabaseEditor.Commands;

public sealed class AddRecipeCommand : Command
{
    private readonly TextWriter _output;
    private readonly RecipeParser _recipeParser;
    private readonly IRecipeRepository _recipeRepository;

    private const string MissingProductsFile = "missing_products.txt";

    private readonly HashSet<string> _missingProducts = new();

    public AddRecipeCommand(TextWriter output, RecipeParser recipeParser, IRecipeRepository recipeRepository)
        : base(new[] { "add", "recipe" },
            "Добавить рецепт[-ы] в базу данных - add recipe <file or folder>")
    {
        _output = output;
        _recipeParser = recipeParser;
        _recipeRepository = recipeRepository;
    }

    public override void Execute(string[] args)
    {
        if (args.Length != 1)
        {
            _output.WriteLine("Неверное количество аргументов");
            return;
        }

        var path = args[0];
        var files = ParseFilesFromInput(path);

        if (files.Count == 0)
        {
            return;
        }

        var recipes = files.Select(TryLoadRecipeFromFile)
            .Where(p => p is not null)
            .ToList();

        _recipeRepository.AddRecipesAsync(recipes!).Wait();
        _output.WriteLine($"Добавлено {recipes.Count} рецептов");
    }

    private List<string> ParseFilesFromInput(string path)
    {
        if (File.Exists(path))
        {
            return new List<string> { path };
        }

        if (Directory.Exists(path))
        {
            return Directory.GetFiles(path, "*.rec", SearchOption.AllDirectories).ToList();
        }

        _output.WriteLine("Неверный путь к файлу или папке");
        return new List<string>();
    }

    private Recipe? TryLoadRecipeFromFile(string filePath)
    {
        var text = File.ReadAllText(filePath);
        Recipe? recipe = null;

        try
        {
            recipe = _recipeParser.TryParseRecipe(text);
        }
        catch (ProductMissingException productMissingException)
        {
            AddMissingProductName(productMissingException.ProductName);
            _output.WriteLine($"Не удалось загрузить рецепт {filePath}: {productMissingException.Message}");
            return null;
        }
        catch (RecipeParsingException e)
        {
            _output.WriteLine($"Не удалось загрузить рецепт {filePath}: {e.Message}");
            return null;
        }

        if (recipe is not null)
        {
            return recipe;
        }

        _output.WriteLine($"Не удалось распознать рецепт в файле {filePath}");
        return null;
    }

    private void AddMissingProductName(string name)
    {
        name = name.ToLower();
        if (_missingProducts.Add(name))
        {
            File.AppendAllText(MissingProductsFile, name + '\n');
        }
    }
}