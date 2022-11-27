using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Recipes.Application.Services.Preferences;

public interface IPreferenceService
{
    Preferences GetPreferences();
    void SavePreferences(Preferences preferences);
}

public class PreferenceService : IPreferenceService
{
    private readonly ILogger<PreferenceService> _logger;

    private readonly string _preferencesPath;
    private Preferences? _preferences;

    public PreferenceService(ILogger<PreferenceService> logger, string preferencesPath)
    {
        _logger = logger;
        _preferencesPath = preferencesPath;
    }

    public Preferences GetPreferences()
    {
        if (_preferences is not null)
        {
            return _preferences;
        }

        var existingPreferences = LoadPreferences();
        if (existingPreferences is null)
        {
            return CreateNewPreferences();
        }

        return _preferences = existingPreferences;
    }

    public void SavePreferences(Preferences preferences)
    {
        _preferences = Guard.Against.Null(preferences);

        _logger.LogInformation("Saving preferences to {Path}", _preferencesPath);

        var json = JsonConvert.SerializeObject(preferences);

        try
        {
            File.WriteAllText(_preferencesPath, json);
        }
        catch (SystemException ex)
        {
            _logger.LogError(ex, "Error saving preferences");
        }
    }


    private Preferences CreateNewPreferences()
    {
        _preferences = new Preferences();
        SavePreferences(_preferences);

        return _preferences;
    }

    private Preferences? LoadPreferences()
    {
        _logger.LogInformation("Loading preferences from {Path}", _preferencesPath);

        if (!File.Exists(_preferencesPath))
        {
            _logger.LogInformation("Preferences file not found");
            return null;
        }

        try
        {
            var json = File.ReadAllText(_preferencesPath);
            return JsonConvert.DeserializeObject<Preferences>(json);
        }
        catch (SystemException ex)
        {
            _logger.LogError(ex, "Error loading preferences");
            return null;
        }
    }
}