using Recipes.Domain.Entities.ProductAggregate;

namespace Recipes.Tests.Shared.Builders;

public class ProductBuilder : BaseEntityBuilder<Product, ProductBuilder>
{
    private string _name;
    private string? _description;
    private Uri? _imageUrl = null;

    public ProductBuilder()
    {
        _name = $"Product Name {Guid.NewGuid()}";
    }

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public ProductBuilder WithImageUrl(Uri? imageUrl)
    {
        _imageUrl = imageUrl;
        return this;
    }

    public override Product Build()
    {
        return new Product(Id, _name)
        {
            Description = _description,
            ImageUrl = _imageUrl
        };
    }
}