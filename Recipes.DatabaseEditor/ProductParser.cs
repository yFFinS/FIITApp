using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.DatabaseEditor;

public class ProductParser
{
    public Product? TryParseProduct(string text)
    {
        var lines = text.Split(Environment.NewLine).Select(l => l.Trim()).ToArray();
        var name = lines[0];
        var description = lines[1] == "-" ? null : lines[1];
        var imageUrl = new Uri(lines[2], UriKind.Absolute);

        return new Product(EntityId.NewId(), name)
        {
            Description = description,
            ImageUrl = imageUrl
        };
    }
}