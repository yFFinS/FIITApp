using System.IO;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Recipes.Application.Services.Preferences;
using Recipes.Tests.Shared.BuilderEntries;

namespace Recipes.Application.UnitTests.Tests;

[TestFixture]
public class PreferenceServiceTests
{
    private const string TestPath = "test.json";

    [SetUp]
    public void Setup()
    {
        DeleteSavedPreferences();
    }

    [TearDown]
    public void TearDown()
    {
        DeleteSavedPreferences();
    }

    private static void DeleteSavedPreferences()
    {
        if (File.Exists(TestPath))
        {
            File.Delete(TestPath);
        }
    }

    private static PreferenceService CreateService()
    {
        var preferencesService = new PreferenceService(NullLogger<PreferenceService>.Instance,
            new PreferenceServiceOptions(TestPath));
        return preferencesService;
    }

    [Test]
    public void Test_CreatesNewPreferences()
    {
        var preferencesService = CreateService();

        var preferences = preferencesService.GetPreferences();
        Assert.That(preferences, Is.Not.Null);
    }

    [Test]
    public void Test_SavesPreferences()
    {
        var preferencesService = CreateService();

        var likedProductId = An.EntityId;
        var dislikedRecipeId = An.EntityId;

        var preferences = new Preferences();
        preferences.LikeProduct(likedProductId);
        preferences.DislikeRecipe(dislikedRecipeId);

        preferencesService.SavePreferences(preferences);

        var savedPreferences = preferencesService.GetPreferences();
        Assert.That(savedPreferences, Is.Not.Null);
        Assert.That(savedPreferences.IsLikedProduct(likedProductId), Is.True);
        Assert.That(savedPreferences.IsDislikedRecipe(dislikedRecipeId), Is.True);
    }
}