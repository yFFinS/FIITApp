using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Recipes.Domain.Interfaces;

namespace Recipes.Infrastructure;

public class OptionsInjector
{
    private readonly string _optionsFolderPath;

    private readonly Dictionary<Type, Func<IOptions>> _factories = new();
    private readonly Dictionary<Type, IOptions> _fixedOptions = new();

    public OptionsInjector(string optionsFolderPath)
    {
        _optionsFolderPath = optionsFolderPath;
    }

    public OptionsInjector AddExternalOptions<T>(Func<T> defaultFactory) where T : IOptions
    {
        _factories.Add(typeof(T), () => defaultFactory());
        return this;
    }

    public OptionsInjector AddFixedOptions<T>(T options) where T : IOptions
    {
        _fixedOptions.Add(typeof(T), options);
        return this;
    }

    private object CreateDefault(Type type, string path)
    {
        var defaultOptions = _factories[type]();
        var defaultOptionsJson = JsonConvert.SerializeObject(defaultOptions, Formatting.Indented);
        File.WriteAllText(path, defaultOptionsJson);

        return defaultOptions;
    }

    private object ReadOrCreateDefaultOptions(Type type, string path)
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
        if (!Directory.Exists(_optionsFolderPath))
        {
            Directory.CreateDirectory(_optionsFolderPath);
        }

        foreach (var (type, options) in _fixedOptions)
        {
            serviceCollection.AddSingleton(type, options);
        }

        foreach (var type in _factories.Keys)
        {
            var filePath = Path.Combine(_optionsFolderPath, $"{type.Name}.json");
            var options = ReadOrCreateDefaultOptions(type, filePath);

            serviceCollection.AddSingleton(type, options);
        }

        return serviceCollection;
    }
}