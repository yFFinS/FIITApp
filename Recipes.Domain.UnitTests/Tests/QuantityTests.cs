using NUnit.Framework;
using Recipes.Domain.UnitTests.BuilderEntries;
using Recipes.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Tests.Tests;

public class QuantityTests
{
    [Test]
    public void Test_OneTypeConvert()
    {
        var a = new Quantity(2, Domain.Enums.QuantityUnit.DessertSpoons);
        var b = a.ConvertTo(Domain.Enums.QuantityUnit.Milliliters);
        var c = new Quantity(20, Domain.Enums.QuantityUnit.Milliliters);
        Assert.AreEqual(b, c);
    }
}

