using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.ProductAggregate;

namespace Recipes.DatabaseEditor.Commands;

public sealed class AddProductCommand : Command
{
    private readonly TextWriter _output;
    private readonly ProductParser _productParser;
    private readonly IProductRepository _productRepository;

    public AddProductCommand(TextWriter output, ProductParser productParser, IProductRepository productRepository)
        : base(new[] { "add", "product" },
            "Добавить продукт[-ы] в базу данных - add product <file or folder>")
    {
        _output = output;
        _productParser = productParser;
        _productRepository = productRepository;
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

        var products = files.Select(TryLoadProductFromFile)
            .Where(p => p is not null)
            .ToList();

        _productRepository.AddProductsAsync(products!).Wait();
        _output.WriteLine($"Добавлено {products.Count} продуктов");
    }

    private List<string> ParseFilesFromInput(string path)
    {
        if (File.Exists(path))
        {
            return new List<string> { path };
        }

        if (Directory.Exists(path))
        {
            return Directory.GetFiles(path, "*.prod", SearchOption.AllDirectories).ToList();
        }

        _output.WriteLine("Неверный путь к файлу или папке");
        return new List<string>();
    }

    private Product? TryLoadProductFromFile(string filePath)
    {
        var text = File.ReadAllText(filePath);
        var product = _productParser.TryParseProduct(text);

        if (product is not null)
        {
            return product;
        }

        _output.WriteLine($"Не удалось распознать продукт в файле {filePath}");
        return null;
    }
}