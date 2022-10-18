// using NUnit.Framework;
// using Recipes.Domain.Enums;
// using Recipes.Domain.Interfaces;
// using Recipes.Domain.Services;
// using Recipes.Domain.ValueObjects;
//
// namespace Recipes.Domain.UnitTests.Tests;
//
// [TestFixture]
// public class ConversionQuantityTests
// {
//     private IQuantityConverter _converter = null!;
//
//     [SetUp]
//     public void SetUp()
//     {
//         _converter = new QuantityConverter();
//         _converter.SetConversionFactor(QuantityUnit.Milliliters, QuantityUnit.TeaSpoons, 5);
//     }
//
//     [TestCase(1, QuantityUnit.Grams, 2, QuantityUnit.Milliliters, 0.5)]
//     [TestCase(5, QuantityUnit.Grams, 5, QuantityUnit.TeaSpoons, 0.2)]
//     [TestCase(12, QuantityUnit.Grams, 4, QuantityUnit.TableSpoons, 0.25)]
//     [TestCase(10, QuantityUnit.Grams, 5, QuantityUnit.DessertSpoons, 0.2)]
//     [TestCase(250, QuantityUnit.Grams, 10, QuantityUnit.Cups, 0.1)]
//     [TestCase(100, QuantityUnit.Grams, 5, QuantityUnit.Decilitres, 0.2)]
//     [TestCase(1000, QuantityUnit.Grams, 1, QuantityUnit.Liters, 1)]
//     [TestCase(1, QuantityUnit.TeaSpoons, 5, QuantityUnit.Grams, 25)]
//     [TestCase(1, QuantityUnit.TableSpoons, 9, QuantityUnit.Grams, 108)]
//     [TestCase(1, QuantityUnit.DessertSpoons, 8, QuantityUnit.Grams, 80)]
//     [TestCase(1, QuantityUnit.Cups, 4, QuantityUnit.Kilograms, 1)]
//     [TestCase(1, QuantityUnit.Decilitres, 3.5, QuantityUnit.Grams, 350)]
//     [TestCase(1, QuantityUnit.Liters, 1.33, QuantityUnit.Kilograms, 1.33)]
//     [TestCase(12.5, QuantityUnit.Grams, 5, QuantityUnit.TeaSpoons, 0.5)]
//     [TestCase(24, QuantityUnit.Grams, 4, QuantityUnit.TableSpoons, 0.5)]
//     [TestCase(15, QuantityUnit.Grams, 5, QuantityUnit.DessertSpoons, 0.3)]
//     [TestCase(300, QuantityUnit.Grams, 6, QuantityUnit.Cups, 0.2)]
//     [TestCase(10, QuantityUnit.Grams, 0.1, QuantityUnit.Decilitres, 1)]
//     [TestCase(500, QuantityUnit.Grams, 0.5, QuantityUnit.Liters, 1)]
//     [TestCase(28, QuantityUnit.TeaSpoons, 0.5, QuantityUnit.Grams, 70)]
//     [TestCase(1.4, QuantityUnit.TableSpoons, 0.125, QuantityUnit.Grams, 2.1)]
//     [TestCase(0.25, QuantityUnit.DessertSpoons, 2, QuantityUnit.Grams, 5)]
//     [TestCase(0.33, QuantityUnit.Cups, 9, QuantityUnit.Grams, 742.5)]
//     [TestCase(10.23, QuantityUnit.Decilitres, 0.7, QuantityUnit.Grams, 716.1)]
//     [TestCase(0.5011, QuantityUnit.Liters, 0.1, QuantityUnit.Grams, 50.11)]
//     public void Test_Quantity_ConvertWithDensity(double value, QuantityUnit unit, double? density,
//         QuantityUnit convertToUnit, double convertedValue)
//     {
//         var quantity = new Quantity(value, unit, density);
//         var convertedQuantity = quantity.ImplicitlyConvertTo(convertToUnit);
//
//         Assert.AreEqual(convertedValue, convertedQuantity.Value);
//     }
//
//     [TestCase(1, QuantityUnit.Grams, 2, QuantityUnit.Milliliters, 0.5)]
//     [TestCase(5, QuantityUnit.Grams, 5, QuantityUnit.TeaSpoons, 0.2)]
//     [TestCase(12, QuantityUnit.Grams, 4, QuantityUnit.TableSpoons, 0.25)]
//     [TestCase(10, QuantityUnit.Grams, 5, QuantityUnit.DessertSpoons, 0.2)]
//     [TestCase(250, QuantityUnit.Grams, 10, QuantityUnit.Cups, 0.1)]
//     [TestCase(100, QuantityUnit.Grams, 5, QuantityUnit.Decilitres, 0.2)]
//     [TestCase(1000, QuantityUnit.Grams, 1, QuantityUnit.Liters, 1)]
//     [TestCase(1, QuantityUnit.TeaSpoons, 5, QuantityUnit.Grams, 25)]
//     [TestCase(1, QuantityUnit.TableSpoons, 9, QuantityUnit.Grams, 108)]
//     [TestCase(1, QuantityUnit.DessertSpoons, 8, QuantityUnit.Grams, 80)]
//     [TestCase(1, QuantityUnit.Cups, 4, QuantityUnit.Kilograms, 1)]
//     [TestCase(1, QuantityUnit.Decilitres, 3.5, QuantityUnit.Grams, 350)]
//     [TestCase(1, QuantityUnit.Liters, 1.33, QuantityUnit.Kilograms, 1.33)]
//     [TestCase(12.5, QuantityUnit.Grams, 5, QuantityUnit.TeaSpoons, 0.5)]
//     [TestCase(24, QuantityUnit.Grams, 4, QuantityUnit.TableSpoons, 0.5)]
//     [TestCase(15, QuantityUnit.Grams, 5, QuantityUnit.DessertSpoons, 0.3)]
//     [TestCase(300, QuantityUnit.Grams, 6, QuantityUnit.Cups, 0.2)]
//     [TestCase(10, QuantityUnit.Grams, 0.1, QuantityUnit.Decilitres, 1)]
//     [TestCase(500, QuantityUnit.Grams, 0.5, QuantityUnit.Liters, 1)]
//     [TestCase(28, QuantityUnit.TeaSpoons, 0.5, QuantityUnit.Grams, 70)]
//     [TestCase(1.4, QuantityUnit.TableSpoons, 0.125, QuantityUnit.Grams, 2.1)]
//     [TestCase(0.25, QuantityUnit.DessertSpoons, 2, QuantityUnit.Grams, 5)]
//     [TestCase(0.33, QuantityUnit.Cups, 9, QuantityUnit.Grams, 742.5)]
//     [TestCase(10.23, QuantityUnit.Decilitres, 0.7, QuantityUnit.Grams, 716.1)]
//     [TestCase(0.5011, QuantityUnit.Liters, 0.1, QuantityUnit.Grams, 50.11)]
//     public void Test_Quantity_Operators(double value1, QuantityUnit unit1, double? density,
//         QuantityUnit unit2, double value2)
//     {
//         var quantity1 = new Quantity(value1, unit1, density);
//
//         var quantity2 = new Quantity(value2, unit2);
//         Assert.True(quantity1 == quantity2);
//
//         var quantity3 = new Quantity(value2 + 1, unit2);
//         Assert.True(quantity1 != quantity3);
//         Assert.True(quantity1 < quantity3);
//         Assert.True(quantity1 <= quantity3);
//         Assert.True(quantity3 > quantity1);
//         Assert.True(quantity3 >= quantity1);
//     }
// }