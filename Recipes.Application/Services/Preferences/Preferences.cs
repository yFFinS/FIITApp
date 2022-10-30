using Newtonsoft.Json;
using Recipes.Domain.ValueObjects;

namespace Recipes.Application.Services.Preferences;

public enum Preference
{
    Unknown,
    Liked,
    Disliked,
}

public class Preferences
{
    [JsonProperty] private readonly Dictionary<EntityId, Preference> _preferences;

    [JsonConstructor]
    private Preferences(Dictionary<EntityId, Preference> preferencesMap)
    {
        _preferences = preferencesMap;
    }

    public Preferences() => _preferences = new Dictionary<EntityId, Preference>();

    private Preference GetPreference(EntityId entityId) => _preferences.GetValueOrDefault(entityId, Preference.Unknown);
    private void SetPreference(EntityId entityId, Preference preference) => _preferences[entityId] = preference;

    public Preference GetProductPreference(EntityId productId) => GetPreference(productId);
    public Preference GetRecipePreference(EntityId recipeId) => GetPreference(recipeId);

    public void DislikeRecipe(EntityId recipeId) => SetPreference(recipeId, Preference.Disliked);
    public void DislikeProduct(EntityId productId) => SetPreference(productId, Preference.Disliked);

    public void LikeProduct(EntityId productId) => SetPreference(productId, Preference.Liked);
    public void LikeRecipe(EntityId recipeId) => SetPreference(recipeId, Preference.Liked);

    public void ResetRecipe(EntityId recipeId) => SetPreference(recipeId, Preference.Unknown);
    public void ResetProduct(EntityId productId) => SetPreference(productId, Preference.Unknown);
}