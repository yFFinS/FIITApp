using Recipes.Domain.ValueObjects;
using System.Xml.Serialization;

namespace Recipes.Infrastructure;

[XmlType(nameof(Quantity))]
public class QuantityDbo
{
    public double Value { get; set; }
    public int UnitId { get; set; }
}