using System;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Recipes.Infrastructure.UnitTests;

[TestFixture]
public class QuantityUnitRepositoryTests
{
    private const string UnitsFilePath = "units.csv";

    private static QuantityUnitRepository CreateRepository(bool cache = true)
    {
        return new QuantityUnitRepository(
            NullLogger<QuantityUnitRepository>.Instance,
            new QuantityUnitRepositoryOptions(UnitsFilePath, CacheUnits: cache));
    }

    [Test]
    public void Test_LoadsUnits()
    {
        var repository = CreateRepository();
        var units = repository.GetAllUnits();

        Assert.That(units.Count, Is.GreaterThan(0));

        foreach (var unit in units)
        {
            Console.WriteLine($"{unit.Names} ({unit.Abbreviations})");
        }
    }
}