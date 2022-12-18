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
    private static IServiceCollection AddScoringCriteria(this IServiceCollection serviceCollection)
    {
        var scoresInjector = new CriteriaScoresInjector(scoresFolderPath: "ScoringCriteria")
            .AddScores(() => new PreferencesCriteriaScores(50, -50, 150, -300))
            .AddScores(() => new FilterCriteriaScores(-30, 50, 150));

        scoresInjector.Inject(serviceCollection);

        serviceCollection.AddTransient<IScoringCriteria, PreferencesScoringCriteria>();
        serviceCollection.AddTransient<IScoringCriteria, FilterScoringCriteria>();
        serviceCollection.AddTransient<IReadOnlyList<IScoringCriteria>>(sp =>
            sp.GetServices<IScoringCriteria>().ToList());

        return serviceCollection;
    }

    public static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddLogging(builder => builder.AddConsole());

        services.AddSingleton<ILogger>(x =>
            x.GetRequiredService<ILoggerFactory>().CreateLogger("Default"));

        services.AddTransient<IRecipeIngredientsMerger, RecipeIngredientsMerger>();
        services.AddTransient<IRecipePicker, RecipePicker>();
        services.AddTransient<IQuantityConverter, QuantityConverter>();
        services.AddTransient<IIngredientGroupEditService, IngredientGroupEditService>();
        services.AddTransient<IShoppingListService, ShoppingListService>();
        services.AddSingleton<IPreferenceService>(sp =>
            new PreferenceService(sp.GetService<ILogger<PreferenceService>>()!,
                "preferences.json"));

        services.AddSingleton<IProductRepository, ProductRepository>();
        services.AddSingleton<IRecipeRepository, RecipeRepository>();
        services.AddSingleton<IImageLoader>(sp =>
            new CachingImageLoader(sp.GetService<ILogger<CachingImageLoader>>()!, cacheSize: 1024));

        services.AddScoringCriteria();

        return services;
    }
}