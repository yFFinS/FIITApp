using Recipes.Domain.IngredientsAggregate;
using System.Xml.Serialization;

namespace Recipes.Infrastructure;

[XmlType(nameof(Ingredient))]
public class IngredientDbo
{
    public string ProductId { get; set; }
    public QuantityDbo QuantityDbo { get; set; }
};