using System.Xml.Serialization;
using Recipes.Domain.ValueObjects;

namespace Recipes.Infrastructure;

[XmlType(nameof(Quantity))]
public class QuantityDbo
{
    public double Value { get; set; }
    public int UnitId { get; set; }
}