using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Recipes.Domain.Enums;
using Recipes.Domain.Exceptions;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.UnitTests.Tests;

public class ValueObjectsTests
{
    #region EnergyValue
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
    #endregion
    #region Quantity
    [TestCase(-1)]
    [TestCase(double.PositiveInfinity)]
    [TestCase(double.NaN)]
    [TestCase(1, 0)]
    [TestCase(1, -0.5)]
    public void Test_Quantity_Throws_WithInvalidValue(double value,
        double? density = null)
    {
        Assert.Throws<ArgumentException>(() => _ = new Quantity(value, QuantityUnit.Cups, density));
    }

    [TestCase(1, QuantityUnit.Pieces, 0.33)]
    [TestCase(25, QuantityUnit.Cups)]
    [TestCase(200, QuantityUnit.Milliliters, 1)]
    [TestCase(3, QuantityUnit.TableSpoons)]
    [TestCase(2, QuantityUnit.TeaSpoons, 0.4)]
    [TestCase(0.5, QuantityUnit.Liters)]
    [TestCase(1.33, QuantityUnit.Kilograms)]
    public void Test_Quantity_Creates_WithValidParameters(double value, QuantityUnit unit,
        double? density = null)
    {
        var quantity = new Quantity(value, unit, density);

        Assert.AreEqual(value, quantity.Value);
        Assert.AreEqual(unit, quantity.Unit);
    }

    [TestCase(QuantityUnit.Cups, QuantityUnit.Kilograms)]
    [TestCase(QuantityUnit.Liters, QuantityUnit.Grams)]
    [TestCase(QuantityUnit.Grams, QuantityUnit.DessertSpoons)]
    [TestCase(QuantityUnit.Kilograms, QuantityUnit.TeaSpoons)]
    public void Test_Quantity_Throws_WithUncomparableException(QuantityUnit unit1, QuantityUnit unit2)
    {
        var quantity1 = new Quantity(5, unit1);
        var quantity2 = new Quantity(10, unit2);
        Assert.Throws<QuantityUncomparableException>(() =>
        _ = quantity1 >= quantity2);
        Assert.Throws<QuantityUncomparableException>(() =>
        _ = quantity1 > quantity2);
        Assert.Throws<QuantityUncomparableException>(() =>
        _ = quantity1 == quantity2);
        Assert.Throws<QuantityUncomparableException>(() =>
        _ = quantity1 != quantity2);
        Assert.Throws<QuantityUncomparableException>(() =>
        _ = quantity1 <= quantity2);
        Assert.Throws<QuantityUncomparableException>(() =>
        _ = quantity1 < quantity2);
    }

    [Test]
    public void Test_Quantity_Throws_WithConversionException()
    {
        var quantity = new Quantity(5, QuantityUnit.Kilograms);
        Assert.Throws<QuantityUnitConversionException>(() => 
        _ = quantity.ConvertTo(QuantityUnit.Cups));
    }

    [Test]
    public void Test_Quantity_Throws_WithMonConvertibleException()
    {
        var quantity = new Quantity(5, QuantityUnit.Pieces);
        Assert.Throws<QuantityUnitNonConvertibleException>(() =>
        _ = quantity.ConvertTo(QuantityUnit.Cups));
    }

    [TestCase(1, QuantityUnit.Milliliters, QuantityUnit.Milliliters, 1)]
    [TestCase(5, QuantityUnit.Milliliters, QuantityUnit.TeaSpoons, 1)]
    [TestCase(12, QuantityUnit.Milliliters, QuantityUnit.TableSpoons, 1)]
    [TestCase(10, QuantityUnit.Milliliters, QuantityUnit.DessertSpoons, 1)]
    [TestCase(250, QuantityUnit.Milliliters, QuantityUnit.Cups, 1)]
    [TestCase(100, QuantityUnit.Milliliters, QuantityUnit.Decilitres, 1)]
    [TestCase(1000, QuantityUnit.Milliliters, QuantityUnit.Liters, 1)]

    [TestCase(1, QuantityUnit.TeaSpoons, QuantityUnit.Milliliters, 5)]
    [TestCase(1, QuantityUnit.TableSpoons, QuantityUnit.Milliliters, 12)]
    [TestCase(1, QuantityUnit.DessertSpoons, QuantityUnit.Milliliters, 10)]
    [TestCase(1, QuantityUnit.Cups, QuantityUnit.Milliliters, 250)]
    [TestCase(1, QuantityUnit.Decilitres, QuantityUnit.Milliliters, 100)]
    [TestCase(1, QuantityUnit.Liters, QuantityUnit.Milliliters, 1000)]

    [TestCase(12.5, QuantityUnit.Milliliters, QuantityUnit.TeaSpoons, 2.5)]
    [TestCase(24, QuantityUnit.Milliliters, QuantityUnit.TableSpoons, 2)]
    [TestCase(15, QuantityUnit.Milliliters, QuantityUnit.DessertSpoons, 1.5)]
    [TestCase(300, QuantityUnit.Milliliters, QuantityUnit.Cups, 1.2)]
    [TestCase(10, QuantityUnit.Milliliters, QuantityUnit.Decilitres, 0.1)]
    [TestCase(500, QuantityUnit.Milliliters, QuantityUnit.Liters, 0.5)]

    [TestCase(28, QuantityUnit.TeaSpoons, QuantityUnit.Milliliters, 140)]
    [TestCase(1.4, QuantityUnit.TableSpoons, QuantityUnit.Milliliters, 16.8)]
    [TestCase(0.25, QuantityUnit.DessertSpoons, QuantityUnit.Milliliters, 2.5)]
    [TestCase(0.33, QuantityUnit.Cups, QuantityUnit.Milliliters, 82.5)]
    [TestCase(10.23, QuantityUnit.Decilitres, QuantityUnit.Milliliters, 1023)]
    [TestCase(0.5011, QuantityUnit.Liters, QuantityUnit.Milliliters, 501.1)]

    [TestCase(1, QuantityUnit.Grams, QuantityUnit.Grams, 1)]
    [TestCase(10, QuantityUnit.Grams, QuantityUnit.Kilograms, 0.01)]

    [TestCase(1.5, QuantityUnit.Kilograms, QuantityUnit.Grams, 1500)]
    public void Test_Quantity_ConvertWithoutDensity(double value, QuantityUnit unit,
        QuantityUnit convertToUnit, double convertedValue)
    {
        var quantity = new Quantity(value, unit);
        var convertedQuantity = quantity.ConvertTo(convertToUnit);

        Assert.AreEqual(convertedValue, convertedQuantity.Value);
    }

    [TestCase(1, QuantityUnit.Grams, 2, QuantityUnit.Milliliters, 0.5)]
    [TestCase(5, QuantityUnit.Grams, 5, QuantityUnit.TeaSpoons, 0.2)]
    [TestCase(12, QuantityUnit.Grams, 4, QuantityUnit.TableSpoons, 0.25)]
    [TestCase(10, QuantityUnit.Grams, 5, QuantityUnit.DessertSpoons, 0.2)]
    [TestCase(250, QuantityUnit.Grams, 10, QuantityUnit.Cups, 0.1)]
    [TestCase(100, QuantityUnit.Grams, 5, QuantityUnit.Decilitres, 0.2)]
    [TestCase(1000, QuantityUnit.Grams, 1, QuantityUnit.Liters, 1)]

    [TestCase(1, QuantityUnit.TeaSpoons, 5, QuantityUnit.Grams, 25)]
    [TestCase(1, QuantityUnit.TableSpoons, 9, QuantityUnit.Grams, 108)]
    [TestCase(1, QuantityUnit.DessertSpoons, 8, QuantityUnit.Grams, 80)]
    [TestCase(1, QuantityUnit.Cups, 4, QuantityUnit.Kilograms, 1)]
    [TestCase(1, QuantityUnit.Decilitres, 3.5, QuantityUnit.Grams, 350)]
    [TestCase(1, QuantityUnit.Liters, 1.33, QuantityUnit.Kilograms, 1.33)]

    [TestCase(12.5, QuantityUnit.Grams, 5, QuantityUnit.TeaSpoons, 0.5)]
    [TestCase(24, QuantityUnit.Grams, 4, QuantityUnit.TableSpoons, 0.5)]
    [TestCase(15, QuantityUnit.Grams, 5, QuantityUnit.DessertSpoons, 0.3)]
    [TestCase(300, QuantityUnit.Grams, 6, QuantityUnit.Cups, 0.2)]
    [TestCase(10, QuantityUnit.Grams, 0.1, QuantityUnit.Decilitres, 1)]
    [TestCase(500, QuantityUnit.Grams, 0.5, QuantityUnit.Liters, 1)]

    [TestCase(28, QuantityUnit.TeaSpoons, 0.5, QuantityUnit.Grams, 70)]
    [TestCase(1.4, QuantityUnit.TableSpoons, 0.125, QuantityUnit.Grams, 2.1)]
    [TestCase(0.25, QuantityUnit.DessertSpoons, 2, QuantityUnit.Grams, 5)]
    [TestCase(0.33, QuantityUnit.Cups, 9, QuantityUnit.Grams, 742.5)]
    [TestCase(10.23, QuantityUnit.Decilitres, 0.7, QuantityUnit.Grams, 716.1)]
    [TestCase(0.5011, QuantityUnit.Liters, 0.1, QuantityUnit.Grams, 50.11)]
    public void Test_Quantity_ConvertWithDensity(double value, QuantityUnit unit, double? density,
        QuantityUnit convertToUnit, double convertedValue)
    {
        var quantity = new Quantity(value, unit, density);
        var convertedQuantity = quantity.ConvertTo(convertToUnit);

        Assert.AreEqual(convertedValue, convertedQuantity.Value);
    }

    [TestCase(1, QuantityUnit.Grams, 2, QuantityUnit.Milliliters, 0.5)]
    [TestCase(5, QuantityUnit.Grams, 5, QuantityUnit.TeaSpoons, 0.2)]
    [TestCase(12, QuantityUnit.Grams, 4, QuantityUnit.TableSpoons, 0.25)]
    [TestCase(10, QuantityUnit.Grams, 5, QuantityUnit.DessertSpoons, 0.2)]
    [TestCase(250, QuantityUnit.Grams, 10, QuantityUnit.Cups, 0.1)]
    [TestCase(100, QuantityUnit.Grams, 5, QuantityUnit.Decilitres, 0.2)]
    [TestCase(1000, QuantityUnit.Grams, 1, QuantityUnit.Liters, 1)]

    [TestCase(1, QuantityUnit.TeaSpoons, 5, QuantityUnit.Grams, 25)]
    [TestCase(1, QuantityUnit.TableSpoons, 9, QuantityUnit.Grams, 108)]
    [TestCase(1, QuantityUnit.DessertSpoons, 8, QuantityUnit.Grams, 80)]
    [TestCase(1, QuantityUnit.Cups, 4, QuantityUnit.Kilograms, 1)]
    [TestCase(1, QuantityUnit.Decilitres, 3.5, QuantityUnit.Grams, 350)]
    [TestCase(1, QuantityUnit.Liters, 1.33, QuantityUnit.Kilograms, 1.33)]

    [TestCase(12.5, QuantityUnit.Grams, 5, QuantityUnit.TeaSpoons, 0.5)]
    [TestCase(24, QuantityUnit.Grams, 4, QuantityUnit.TableSpoons, 0.5)]
    [TestCase(15, QuantityUnit.Grams, 5, QuantityUnit.DessertSpoons, 0.3)]
    [TestCase(300, QuantityUnit.Grams, 6, QuantityUnit.Cups, 0.2)]
    [TestCase(10, QuantityUnit.Grams, 0.1, QuantityUnit.Decilitres, 1)]
    [TestCase(500, QuantityUnit.Grams, 0.5, QuantityUnit.Liters, 1)]

    [TestCase(28, QuantityUnit.TeaSpoons, 0.5, QuantityUnit.Grams, 70)]
    [TestCase(1.4, QuantityUnit.TableSpoons, 0.125, QuantityUnit.Grams, 2.1)]
    [TestCase(0.25, QuantityUnit.DessertSpoons, 2, QuantityUnit.Grams, 5)]
    [TestCase(0.33, QuantityUnit.Cups, 9, QuantityUnit.Grams, 742.5)]
    [TestCase(10.23, QuantityUnit.Decilitres, 0.7, QuantityUnit.Grams, 716.1)]
    [TestCase(0.5011, QuantityUnit.Liters, 0.1, QuantityUnit.Grams, 50.11)]
    public void Test_Quantity_Operators(double value1, QuantityUnit unit1, double? density,
        QuantityUnit unit2, double value2)
    {
        var quantity1 = new Quantity(value1, unit1, density);

        var quantity2 = new Quantity(value2, unit2);
        Assert.True(quantity1 == quantity2);

        var quantity3 = new Quantity(value2 + 1, unit2);
        Assert.True(quantity1 != quantity3);
        Assert.True(quantity1 < quantity3);
        Assert.True(quantity1 <= quantity3);
        Assert.True(quantity3 > quantity1);
        Assert.True(quantity3 >= quantity1);
    }

    public void Test_Quantity_Equality()
    {
        var quantity1 = new Quantity(1, QuantityUnit.Milliliters);

        var quantity2 = new Quantity(1, QuantityUnit.Milliliters, 5);
        Assert.False(quantity1.Equals(quantity2));

        var quantity3 = new Quantity(1, QuantityUnit.Milliliters);
        Assert.True(quantity1.Equals(quantity3));

        var quantity4 = new Quantity(2, QuantityUnit.Milliliters);
        Assert.False(quantity1.Equals(quantity4));
    }
    #endregion
    #region EntityId
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
    #endregion
}