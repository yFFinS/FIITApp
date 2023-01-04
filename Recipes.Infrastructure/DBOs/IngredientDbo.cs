using System.Xml.Serialization;
using Recipes.Domain.IngredientsAggregate;

namespace Recipes.Infrastructure;

[XmlType(nameof(Ingredient))]
public class IngredientDbo
{
    public string ProductId { get; set; }
    public QuantityDbo QuantityDbo { get; set; }
};