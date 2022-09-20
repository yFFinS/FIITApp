using System;
using NUnit.Framework;
using Recipes.Domain.Enums;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.UnitTests.Tests;

public class ValueObjectsTests
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

    [TestCase(-1)]
    [TestCase(double.PositiveInfinity)]
    [TestCase(double.NaN)]
    public void Test_Quality_Throws_WithInvalidValue(double value)
    {
        Assert.Throws<ArgumentException>(() => _ = new Quantity(value, QuantityUnit.Cups));
    }

    [Test]
    public void Test_Quantity_Creates_WithValidParameters()
    {
        var quantity = new Quantity(2, QuantityUnit.Pieces);

        Assert.AreEqual(2, quantity.Value);
        Assert.AreEqual(QuantityUnit.Pieces, quantity.Unit);
    }

    [Test]
    public void Test_Quantity_Equals()
    {
        var quantity1 = new Quantity(2, QuantityUnit.Pieces);
        var quantity2 = new Quantity(2, QuantityUnit.Pieces);
        var quantity3 = new Quantity(3, QuantityUnit.Pieces);
        var quantity4 = new Quantity(2, QuantityUnit.Grams);

        Assert.AreEqual(quantity1, quantity2);
        Assert.AreNotEqual(quantity1, quantity3);
        Assert.AreNotEqual(quantity1, quantity4);
    }

    [Test]
    public void Test_EntityId_Throws_WithNullId()
    {
#pragma warning disable CS8625
        Assert.Throws<ArgumentNullException>((() => _ = new EntityId(null)));
#pragma warning restore CS8625
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase("    a     ")]
    [TestCase("NOT-GUID-STRING")]
    public void Test_EntityId_Throws_WithInvalidId(string id)
    {
        Assert.Throws<FormatException>((() => _ = new EntityId(id)));
    }

    [Test]
    public void Test_EntityId_Creates_WithValidId()
    {
        var id = Guid.NewGuid().ToString();
        var entityId = new EntityId(id);

        Assert.AreEqual(id, entityId.Value.ToString());
    }

    [Test]
    public void Test_EntityId_Equals()
    {
        var id1 = Guid.NewGuid().ToString();
        var id2 = Guid.NewGuid().ToString();

        var entityId1 = new EntityId(id1);
        var entityId2 = new EntityId(id1);
        var entityId3 = new EntityId(id2);

        Assert.AreEqual(entityId1, entityId2);
        Assert.AreNotEqual(entityId1, entityId3);
    }
}