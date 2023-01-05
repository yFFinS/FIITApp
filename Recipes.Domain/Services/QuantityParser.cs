using System.Globalization;
using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.Services;

public interface IQuantityParser
{
    QuantityUnit? TryParseUnit(string unitNameOrAbbreviation);
    Quantity? TryParseQuantity(string quantity);
}

public class QuantityParser : IQuantityParser
{
    private readonly IQuantityUnitRepository _quantityUnitRepository;

    public QuantityParser(IQuantityUnitRepository quantityUnitRepository)
    {
        _quantityUnitRepository = quantityUnitRepository;
    }

    public QuantityUnit? TryParseUnit(string unitNameOrAbbreviation)
    {
        unitNameOrAbbreviation = unitNameOrAbbreviation.Replace('c', 'с').ToLower();

        var units = _quantityUnitRepository.GetAllUnits();
        return units.FirstOrDefault(u =>
            u.Names.Contains(unitNameOrAbbreviation) || u.Abbreviations.Contains(unitNameOrAbbreviation));
    }

    public Quantity? TryParseQuantity(string quantity)
    {
        var unitWithoutValue = TryParseUnit(quantity);
        if (unitWithoutValue is not null)
        {
            return new Quantity(0, unitWithoutValue);
        }

        var quantityParts = quantity.Split(' ', 2);

        var unit = TryParseUnit(quantityParts[1]);
        if (unit == null)
        {
            return null;
        }

        return double.TryParse(quantityParts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var amount)
            ? new Quantity(amount, unit)
            : null;
    }
}