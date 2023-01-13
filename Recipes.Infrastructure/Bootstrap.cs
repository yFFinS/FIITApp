using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Application.Services;
using Recipes.Application.Services.Preferences;
using Recipes.Application.Services.RecipePicker;
using Recipes.Application.Services.RecipePicker.ScoringCriteria;
using Recipes.Domain.Interfaces;
using Recipes.Domain.Services;

namespace Recipes.Infrastructure;

public static class Bootstrap
{
    private static IServiceCollection AddScoringCriteria(this IServiceCollection serviceCollection,
        OptionsInjector optionsInjector)
    {
        optionsInjector
            .AddExternalOptions(() => new PreferencesCriteriaScores(50, -50, 150, -300))
            .AddExternalOptions(() => new FilterCriteriaScores(-30, 50, 150));

        serviceCollection.AddTransient<IScoringCriteria, PreferencesScoringCriteria>();
        serviceCollection.AddTransient<IScoringCriteria, FilterScoringCriteria>();
        serviceCollection.AddTransient<IReadOnlyList<IScoringCriteria>>(sp =>
            sp.GetServices<IScoringCriteria>().ToList());

        return serviceCollection;
    }

    public static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();
        var optionsInjector = new OptionsInjector("Options");

#if DEBUG
        const LogLevel loggingLevel = LogLevel.Debug;
#else
        const LogLevel loggingLevel = LogLevel.Information;
#endif
        services.AddLogging(builder => builder.AddConsole()
            .SetMinimumLevel(loggingLevel));

        services.AddSingleton<ILogger>(x =>
            x.GetRequiredService<ILoggerFactory>().CreateLogger("Default"));

        services.AddTransient<IRecipeIngredientsMerger, RecipeIngredientsMerger>();
        services.AddTransient<IRecipePicker, RecipePicker>();
        services.AddTransient<IQuantityParser, QuantityParser>();
        services.AddTransient<IIngredientGroupEditService, IngredientGroupEditService>();
        services.AddTransient<IShoppingListService, ShoppingListService>();

        optionsInjector.AddFixedOptions(new PreferenceServiceOptions("preferences.json"));
        services.AddSingleton<IPreferenceService, PreferenceService>();

        var productsPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            + Path.GetFullPath("/")[2..]
            + "Products.xml";

        var recipesPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            + Path.GetFullPath("/")[2..]
            + "Recipes.xml";

        optionsInjector.AddFixedOptions(new DataBaseOptions(productsPath, recipesPath));
        services.AddSingleton<IDataBase, DataBase>();

        services.AddSingleton<IProductRepository, ProductRepository>();
        services.AddSingleton<IRecipeRepository, RecipeRepository>();

        optionsInjector.AddFixedOptions(new QuantityUnitRepositoryOptions("units.csv", CacheUnits: true));
        services.AddSingleton<IQuantityUnitRepository, QuantityUnitRepository>();

        optionsInjector.AddFixedOptions(new CachingImageLoaderOptions(1024));
        services.AddSingleton<IImageLoader, CachingImageLoader>();

        services.AddScoringCriteria(optionsInjector);

        optionsInjector.Inject(services);
        return services;
    }
}