using CsvHelper;
using Recipes.Domain.Interfaces;
using System.Globalization;

namespace Recipes.Infrastructure;

public record ProductNameUnifierOptions(string ProductMappingFile) : IOptions;

public class ProductNameUnifier : IProductNameUnifier
{
    private readonly ProductNameUnifierOptions _options;

    private readonly Dictionary<string, string> _productMapping = new();

    public ProductNameUnifier(ProductNameUnifierOptions options)
    {
        _options = options;
    }

    private void InitProductMapping()
    {
        if (_productMapping.Count > 0)
        {
            return;
        }

        using var reader = new StreamReader(_options.ProductMappingFile);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Read();

        while (csv.Read())
        {
            var productName = csv.GetField<string>(0)!;
            var unifiedName = csv.GetField<string>(1)!;
            _productMapping.Add(productName, unifiedName);
        }
    }

    public string GetUnifiedName(string productName)
    {
        InitProductMapping();

        return _productMapping.TryGetValue(productName.ToLower(), out var unifiedName) ? unifiedName : productName;
    }
}