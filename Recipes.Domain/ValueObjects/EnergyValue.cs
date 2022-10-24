using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Shared;

namespace Recipes.Domain.ValueObjects;

public sealed class EnergyValue : ValueObject<EnergyValue>
{
    public double Calories { get; }
    public double Proteins { get; }
    public double Fats { get; }
    public double Carbohydrates { get; }

    public EnergyValue(double calories, double proteins, double fats, double carbohydrates)
    {
        Calories = Guard.Against.NegativeOrInvalid(calories);
        Proteins = Guard.Against.NegativeOrInvalid(proteins);
        Fats = Guard.Against.NegativeOrInvalid(fats);
        Carbohydrates = Guard.Against.NegativeOrInvalid(carbohydrates);
    }
}