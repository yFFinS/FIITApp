using Ardalis.GuardClauses;

namespace Recipes.Domain.ValueObjects;

public class EnergyValue
{
    public readonly double Calories;
    public readonly double Protein;
    public readonly double Fat;
    public readonly double Carbohydrates;

    public EnergyValue(double calories, double protein, double fat, double carbohydrates)
    {
        Guard.Against.Negative(calories);
        Guard.Against.Negative(protein);
        Guard.Against.Negative(fat);
        Guard.Against.Negative(carbohydrates);

        Calories = calories;
        Protein = protein;
        Fat = fat;
        Carbohydrates = carbohydrates;
    }
}