using System;
using NUnit.Framework;
using Recipes.Domain.Enums;
using Recipes.Domain.Exceptions;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.UnitTests.Tests;

[TestFixture]
public class ElementaryQuantityTests
{
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
    public void Test_Quantity_Throws_WithIncomparableException(QuantityUnit unit1, QuantityUnit unit2)
    {
        var quantity1 = new Quantity(5, unit1);
        var quantity2 = new Quantity(10, unit2);
        Assert.Throws<QuantityIncomparableException>(() =>
            _ = quantity1 >= quantity2);
        Assert.Throws<QuantityIncomparableException>(() =>
            _ = quantity1 > quantity2);
        Assert.Throws<QuantityIncomparableException>(() =>
            _ = quantity1 == quantity2);
        Assert.Throws<QuantityIncomparableException>(() =>
            _ = quantity1 != quantity2);
        Assert.Throws<QuantityIncomparableException>(() =>
            _ = quantity1 <= quantity2);
        Assert.Throws<QuantityIncomparableException>(() =>
            _ = quantity1 < quantity2);
    }

    [Test]
    public void Test_Quantity_Throws_WithConversionException()
    {
        var quantity = new Quantity(5, QuantityUnit.Kilograms);
        Assert.Throws<QuantityUnitConversionException>(() =>
            _ = quantity.ImplicitlyConvertTo(QuantityUnit.Cups));
    }

    [Test]
    public void Test_Quantity_Throws_WithMonConvertibleException()
    {
        var quantity = new Quantity(5, QuantityUnit.Pieces);
        Assert.Throws<QuantityUnitNonConvertibleException>(() =>
            _ = quantity.ImplicitlyConvertTo(QuantityUnit.Cups));
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
        var convertedQuantity = quantity.ImplicitlyConvertTo(convertToUnit);

        Assert.AreEqual(convertedValue, convertedQuantity.Value);
    }
}