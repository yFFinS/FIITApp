using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Recipes.Application.Services.RecipePicker.ScoringCriteria;

namespace Recipes.Infrastructure;

public class CriteriaScoresInjector
{
    private readonly string _scoresFolderPath;
    private readonly Dictionary<Type, Func<ICriteriaScores>> _factories = new();

    public CriteriaScoresInjector(string scoresFolderPath)
    {
        _scoresFolderPath = scoresFolderPath;
    }

    public CriteriaScoresInjector AddScores<T>(Func<T> defaultFactory) where T : ICriteriaScores
    {
        _factories.Add(typeof(T), () => defaultFactory());
        return this;
    }

    private object CreateDefault(Type type, string path)
    {
        var defaultScores = _factories[type]();
        var defaultScoresJson = JsonConvert.SerializeObject(defaultScores, Formatting.Indented);
        File.WriteAllText(path, defaultScoresJson);

        return defaultScores;
    }

    private object ReadOrCreateDefaultScores(Type type, string path)
    {
        if (!File.Exists(path))
        {
            return CreateDefault(type, path);
        }

        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject(json, type) ?? CreateDefault(type, path);
    }

    public IServiceCollection Inject(IServiceCollection serviceCollection)
    {
        if (!Directory.Exists(_scoresFolderPath))
        {
            Directory.CreateDirectory(_scoresFolderPath);
        }

        foreach (var type in _factories.Keys)
        {
            var filePath = Path.Combine(_scoresFolderPath, $"{type.Name}.json");
            var scores = ReadOrCreateDefaultScores(type, filePath);

            serviceCollection.AddSingleton(type, scores);
        }

        return serviceCollection;
    }
}