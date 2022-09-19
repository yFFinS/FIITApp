using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Shared;

namespace Recipes.Domain.ValueObjects;

public sealed class EnergyValue : ValueObject
{
    public readonly double Calories;
    public readonly double Proteins;
    public readonly double Fats;
    public readonly double Carbohydrates;

    public EnergyValue(double calories, double proteins, double fats, double carbohydrates)
    {
        Calories = Guard.Against.NegativeOrInvalid(calories);
        Proteins = Guard.Against.NegativeOrInvalid(proteins);
        Fats = Guard.Against.NegativeOrInvalid(fats);
        Carbohydrates = Guard.Against.NegativeOrInvalid(carbohydrates);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Calories;
        yield return Proteins;
        yield return Fats;
        yield return Carbohydrates;
    }
}