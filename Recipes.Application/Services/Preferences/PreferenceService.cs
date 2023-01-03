using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Recipes.Domain.Interfaces;

namespace Recipes.Application.Services.Preferences;

public interface IPreferenceService
{
    Preferences GetPreferences();
    void SavePreferences(Preferences preferences);
}

public record PreferenceServiceOptions(string PreferencesFilePath) : IOptions;

public class PreferenceService : IPreferenceService
{
    private readonly ILogger<PreferenceService> _logger;

    private readonly PreferenceServiceOptions _options;
    private Preferences? _preferences;

    public PreferenceService(ILogger<PreferenceService> logger, PreferenceServiceOptions options)
    {
        _logger = logger;
        _options = options;
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

        _logger.LogInformation("Saving preferences to {Path}", _options.PreferencesFilePath);

        var json = JsonConvert.SerializeObject(preferences);

        try
        {
            File.WriteAllText(_options.PreferencesFilePath, json);
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
        _logger.LogInformation("Loading preferences from {Path}", _options.PreferencesFilePath);

        if (!File.Exists(_options.PreferencesFilePath))
        {
            _logger.LogInformation("Preferences file not found");
            return null;
        }

        try
        {
            var json = File.ReadAllText(_options.PreferencesFilePath);
            return JsonConvert.DeserializeObject<Preferences>(json);
        }
        catch (SystemException ex)
        {
            _logger.LogError(ex, "Error loading preferences");
            return null;
        }
    }
}