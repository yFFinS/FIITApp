using Ardalis.GuardClauses;
using Recipes.Domain.Base;

namespace Recipes.Domain.ValueObjects;

public class QuantityUnit : ValueObject<QuantityUnit>
{
    public string Name { get; }
    public string Abbreviation { get; }
    public double? GramsConversionFactor { get; }
    public double? MillilitersConversionFactor { get; }

    public QuantityUnit(string name, string abbreviation,
        double? gramsConversionFactor = null, double? millilitersConversionFactor = null)
    {
        Name = Guard.Against.NullOrEmpty(name);
        Abbreviation = Guard.Against.NullOrEmpty(abbreviation);
        GramsConversionFactor = gramsConversionFactor;
        MillilitersConversionFactor = millilitersConversionFactor;
    }
}