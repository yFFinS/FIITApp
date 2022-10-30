using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Application.Services;
using Recipes.Application.Services.Preferences;
using Recipes.Application.Services.RecipePicker;
using Recipes.Domain.Interfaces;
using Recipes.Domain.Services;

namespace Recipes.Infrastructure;

public static class Bootstrap
{
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
        services.AddSingleton<IPreferenceService>(provider =>
            new PreferenceService(provider.GetService<ILogger<PreferenceService>>()!,
                "preferences.json"));

        return services;
    }
}