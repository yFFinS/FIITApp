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
        var units = _quantityUnitRepository.GetAllUnits();
        return units.FirstOrDefault(u =>
            u.Name == unitNameOrAbbreviation || u.Abbreviation == unitNameOrAbbreviation);
    }

    public Quantity? TryParseQuantity(string quantity)
    {
        var quantityParts = quantity.Split(' ');
        if (quantityParts.Length != 2)
        {
            return null;
        }

        var unit = TryParseUnit(quantityParts[1]);
        if (unit == null)
        {
            return null;
        }

        return double.TryParse(quantityParts[0], out var amount) ? new Quantity(amount, unit) : null;
    }
}