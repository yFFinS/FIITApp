using NUnit.Framework;
using Recipes.Domain.ValueObjects;
using System;

namespace Recipes.Domain.UnitTests.Tests;

[TestFixture]
public class EnergyValueTests
{
    [TestCase(10, 10, 10, -1)]
    [TestCase(-1, 10, 10, 10)]
    [TestCase(10, 10, -1, 10)]
    [TestCase(10, -1, 10, 10)]
    [TestCase(10, 10, double.NaN, 10)]
    [TestCase(double.NegativeInfinity, 10, 1, 10)]
    [TestCase(100, 10, 1, double.NegativeInfinity)]
    public void Test_EnergyValue_Throws_WithInvalidParameters(double calories,
        double proteins, double fats, double carbohydrates)
    {
        Assert.Throws<ArgumentException>(() => _ = new EnergyValue(calories, proteins, fats, carbohydrates));
    }

    [Test]
    public void Test_EnergyValue_Creates_WithValidParameters()
    {
        var energyValue = new EnergyValue(1, 2, 3.4, 4);

        Assert.AreEqual(1, energyValue.Calories);
        Assert.AreEqual(2, energyValue.Proteins);
        Assert.AreEqual(3.4, energyValue.Fats);
        Assert.AreEqual(4, energyValue.Carbohydrates);
    }

    [Test]
    public void Test_EnergyValue_Equals()
    {
        var energyValue1 = new EnergyValue(1, 2, 3.4, 4);
        var energyValue2 = new EnergyValue(1, 2, 3.4, 4);
        var energyValue3 = new EnergyValue(1, 2, 3.4, 5);

        Assert.AreEqual(energyValue1, energyValue2);
        Assert.AreNotEqual(energyValue1, energyValue3);
    }
}