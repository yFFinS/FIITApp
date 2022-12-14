using System.Xml.Serialization;
using Recipes.Domain.Entities.ProductAggregate;

namespace Recipes.Infrastructure;

[XmlType(nameof(Product))]
public class ProductDbo : EntityDbo<ProductDbo>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public double? PieceWeight { get; set; }
    public string? ImageUrl { get; set; }
    public int[] QuantityUnitIds { get; set; }
}