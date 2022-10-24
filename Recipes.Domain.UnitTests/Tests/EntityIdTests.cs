using System;
using NUnit.Framework;
using Recipes.Domain.ValueObjects;

namespace Recipes.Domain.UnitTests.Tests;

[TestFixture]
public class EntityIdTests
{
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

    [Test]
    public void Test_EntityId_GetHashCode()
    {
        var id1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var id2 = Guid.Parse("00000000-0000-0000-0000-000000000002");

        var entityId1 = new EntityId(id1);
        var entityId2 = new EntityId(id1);
        var entityId3 = new EntityId(id2);

        Assert.AreEqual(entityId1.GetHashCode(), entityId2.GetHashCode());
        Assert.AreNotEqual(entityId1.GetHashCode(), entityId3.GetHashCode());
    }
}