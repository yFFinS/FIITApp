using Ardalis.GuardClauses;
using Recipes.Domain.Base;

namespace Recipes.Domain.ValueObjects;

public class QuantityUnit : ValueObject<QuantityUnit>
{
    public QuantityNames Names { get; }
    public QuantityNames Abbreviations { get; }
    public double? GramsConversionFactor { get; }
    public double? MillilitersConversionFactor { get; }
    public bool IsUniversal { get; }

    public QuantityUnit(string name, string abbreviation,
        double? gramsConversionFactor = null, double? millilitersConversionFactor = null,
        bool isUniversal = false) : this(new QuantityNames(name),
        new QuantityNames(abbreviation), gramsConversionFactor, millilitersConversionFactor, isUniversal)
    {
    }

    public QuantityUnit(QuantityNames names, QuantityNames abbreviations,
        double? gramsConversionFactor = null, double? millilitersConversionFactor = null,
        bool isUniversal = false)
    {
        Names = Guard.Against.Null(names);
        Abbreviations = Guard.Against.Null(abbreviations);

        GramsConversionFactor = gramsConversionFactor;
        MillilitersConversionFactor = millilitersConversionFactor;
        IsUniversal = isUniversal;
    }

    public bool CanConvertTo(QuantityUnit quantityUnit)
    {
        ArgumentNullException.ThrowIfNull(quantityUnit);

        if (!quantityUnit.IsUniversal)
        {
            return false;
        }

        return (GramsConversionFactor.HasValue && quantityUnit.GramsConversionFactor.HasValue) ||
               (MillilitersConversionFactor.HasValue && quantityUnit.MillilitersConversionFactor.HasValue);
    }

    public string GetName(double quantity) => Names.GetQuantityName(quantity);
    public string GetAbbreviation(double quantity) => Abbreviations.GetQuantityName(quantity);
}