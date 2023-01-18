using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Application.Services;
using Recipes.Application.Services.Preferences;
using Recipes.Application.Services.RecipePicker;
using Recipes.Application.Services.RecipePicker.ScoringCriteria;
using Recipes.Domain.Interfaces;
using Recipes.Domain.Services;
using Recipes.Infrastructure.DataBase;
using Recipes.Infrastructure.Repositories;

namespace Recipes.Infrastructure;

public static class Bootstrap
{
    private static IServiceCollection AddScoringCriteria(this IServiceCollection serviceCollection,
        OptionsInjector optionsInjector)
    {
        optionsInjector
            .AddExternalOptions(() => new PreferencesCriteriaScores(50, -50, 150, -300))
            .AddExternalOptions(() => new FilterCriteriaScores(-30, 50, 150, -25))
            .AddExternalOptions(() => new SimplicityCriteriaScores(5, 10.0, 900, 0.1));

        serviceCollection.AddTransient<IScoringCriteria, PreferencesScoringCriteria>();
        serviceCollection.AddTransient<IScoringCriteria, FilterScoringCriteria>();
        serviceCollection.AddTransient<IScoringCriteria, SimplicityScoringCriteria>();
        serviceCollection.AddTransient<IReadOnlyList<IScoringCriteria>>(sp =>
            sp.GetServices<IScoringCriteria>().ToList());

        return serviceCollection;
    }

    private static IServiceCollection ConfigureLogging(this IServiceCollection serviceCollection)
    {
#if DEBUG
        const LogLevel loggingLevel = LogLevel.Debug;
#else
        const LogLevel loggingLevel = LogLevel.Information;
#endif
        serviceCollection.AddLogging(builder => builder.AddConsole()
            .SetMinimumLevel(loggingLevel));

        return serviceCollection;
    }

    public static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();
        var optionsInjector = new OptionsInjector("Options");

        services.ConfigureLogging();
        services.AddSingleton<ILogger>(x =>
            x.GetRequiredService<ILoggerFactory>().CreateLogger("Default"));

        services.AddTransient<IRecipeIngredientsMerger, RecipeIngredientsMerger>();
        services.AddTransient<IRecipePicker, RecipePicker>();
        services.AddTransient<IQuantityParser, QuantityParser>();
        services.AddTransient<IIngredientGroupEditService, IngredientGroupEditService>();

        services.AddSingleton<IPreferenceService, PreferenceService>()
            .AddSingleton(new PreferenceServiceOptions("preferences.json"));

        services.AddSingleton<DatabasePathsProvider>()
            .AddSingleton(new DatabasePaths("Products.xml", "Recipes.xml", "CustomRecipes.xml"));

        services.AddSingleton<FtpService>();
        services.AddSingleton<IDataBase, DataBase.DataBase>();

        services.AddSingleton<IProductRepository, ProductRepository>();
        services.AddSingleton<IRecipeRepository, RecipeRepository>();

        services.AddSingleton<IQuantityUnitRepository, QuantityUnitRepository>()
            .AddSingleton(new QuantityUnitRepositoryOptions("units.csv", CacheUnits: true));

        services.AddSingleton<IImageLoader, CachingImageLoader>()
            .AddSingleton(new CachingImageLoaderOptions(1024));

        services.AddScoringCriteria(optionsInjector);

        optionsInjector.Inject(services);
        return services;
    }
}