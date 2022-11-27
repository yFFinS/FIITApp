using Recipes.Domain.ValueObjects;

namespace Recipes.Application.Services.Preferences;

public static class PreferencesExtensions
{
    public static bool IsLikedRecipe(this Preferences preferences, EntityId recipeId)
    {
        return preferences.GetRecipePreference(recipeId) == Preference.Liked;
    }

    public static bool IsDislikedRecipe(this Preferences preferences, EntityId recipeId)
    {
        return preferences.GetRecipePreference(recipeId) == Preference.Disliked;
    }

    public static bool IsLikedProduct(this Preferences preferences, EntityId productId)
    {
        return preferences.GetProductPreference(productId) == Preference.Liked;
    }

    public static bool IsDislikedProduct(this Preferences preferences, EntityId productId)
    {
        return preferences.GetProductPreference(productId) == Preference.Disliked;
    }
}