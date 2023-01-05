using Recipes.Domain.Base;

namespace Recipes.Domain.ValueObjects;

public class QuantityNames : ValueObject<QuantityNames>
{
    public string Singular { get; }
    public string SemiPlural { get; }
    public string Plural { get; }

    public QuantityNames(string name) : this(name, name, name)
    {
    }

    public QuantityNames(string singular, string semiPlural, string plural)
    {
        Singular = singular;
        SemiPlural = semiPlural;
        Plural = plural;
    }

    public string GetQuantityName(double quantity)
    {
        // 0 is plural
        if (Math.Abs(quantity) < 0.0001)
        {
            return Plural;
        }

        // Not whole number
        if (quantity % 1 != 0)
        {
            return SemiPlural;
        }

        var whole = (int)quantity;

        var tenRemainder = whole % 10;
        var hundredRemainder = whole % 100;

        return tenRemainder switch
        {
            // 1 or 21, 31, 41, 51, 61, 71, 81, 91 etc.
            1 when hundredRemainder != 11 => Singular,
            // 2-4 or 22-24, 32-34, 42-44, 52-54, 62-64, 72-74, 82-84, 92-94 etc.
            >= 2 and <= 4 when hundredRemainder is < 10 or >= 20 => SemiPlural,
            _ => Plural
        };
    }

    public bool Contains(string name)
    {
        return Singular.Equals(name, StringComparison.OrdinalIgnoreCase) ||
               SemiPlural.Equals(name, StringComparison.OrdinalIgnoreCase) ||
               Plural.Equals(name, StringComparison.OrdinalIgnoreCase);
    }
}