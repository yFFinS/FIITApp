using NUnit.Framework;
using Recipes.Application.Services.Preferences;
using Recipes.Tests.Shared.BuilderEntries;

namespace Recipes.Application.UnitTests.Tests;

[TestFixture]
public class PreferencesTests
{
    [Test]
    public void Test_LikeProduct()
    {
        var id = An.EntityId;
        var preferences = new Preferences();
        preferences.LikeProduct(id);

        Assert.That(preferences.IsLikedProduct(id), Is.True);
    }

    [Test]
    public void Test_DislikeProduct()
    {
        var id = An.EntityId;
        var preferences = new Preferences();
        preferences.DislikeProduct(id);

        Assert.That(preferences.IsDislikedProduct(id), Is.True);
    }

    [Test]
    public void Test_LikeProductTwice()
    {
        var id = An.EntityId;
        var preferences = new Preferences();
        preferences.LikeProduct(id);
        preferences.LikeProduct(id);

        Assert.That(preferences.IsLikedProduct(id), Is.True);
    }

    [Test]
    public void Test_LikeAndResetRecipe()
    {
        var id = An.EntityId;
        var preferences = new Preferences();
        preferences.LikeRecipe(id);
        preferences.ResetRecipe(id);

        Assert.That(preferences.IsLikedRecipe(id), Is.False);
        Assert.That(preferences.IsDislikedRecipe(id), Is.False);
    }
}