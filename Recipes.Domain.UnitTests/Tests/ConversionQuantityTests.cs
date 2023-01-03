// using NUnit.Framework;
// using NUnit.Framework.Constraints;
// using Recipes.Domain.Interfaces;
// using Recipes.Domain.Services;
// using Recipes.Domain.ValueObjects;
// using Recipes.Tests.Shared.BuilderEntries;
//
// namespace Recipes.Domain.UnitTests.Tests;
//
// [TestFixture]
// public class ConversionQuantityTests
// {
//     [Test]
//     public void Test_CanConvert_When_HasConversionFactor()
//     {
//         var id = An.EntityId;
//
//         var converter = new QuantityConverter();
//         converter.SetConversionFactor(id, 1.5);
//
//         Assert.That(converter.CanConvert(QuantityUnit.Pieces, QuantityUnit.Grams, id), Is.True);
//     }
//
//     [Test]
//     public void Test_CannotConvert_When_HasNoConversionFactor()
//     {
//         var id = An.EntityId;
//         var otherId = An.EntityId;
//
//         var converter = new QuantityConverter();
//         converter.SetConversionFactor(id, 1.5);
//
//         Assert.That(converter.CanConvert(QuantityUnit.Pieces, QuantityUnit.Grams, otherId), Is.False);
//     }
//
//     [Test]
//     public void Test_CannotConvert_When_WrongUnits()
//     {
//         var id = An.EntityId;
//
//         var converter = new QuantityConverter();
//         converter.SetConversionFactor(id, 1.5);
//
//         Assert.That(converter.CanConvert(QuantityUnit.Grams, QuantityUnit.Cups, id), Is.False);
//     }
//
//     [Test]
//     public void Test_Convert_When_HasConversionFactor()
//     {
//         var id = An.EntityId;
//
//         var converter = new QuantityConverter();
//         converter.SetConversionFactor(id, 1.5);
//
//         var quantity = new Quantity(10.0, QuantityUnit.Pieces);
//         var convertedQuantity = converter.Convert(quantity, QuantityUnit.Grams, id);
//
//         Assert.That(convertedQuantity, Is.EqualTo(new Quantity(15.0, QuantityUnit.Grams)));
//     }
//
//     [Test]
//     public void Test_Convert_To_Kilograms()
//     {
//         var id = An.EntityId;
//
//         var converter = new QuantityConverter();
//         converter.SetConversionFactor(id, 1.5);
//
//         var quantity = new Quantity(1000.0, QuantityUnit.Pieces);
//         var convertedQuantity = converter.Convert(quantity, QuantityUnit.Kilograms, id);
//
//         Assert.That(convertedQuantity, Is.EqualTo(new Quantity(1.5, QuantityUnit.Kilograms)));
//     }
// }