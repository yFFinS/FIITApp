using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Application.Services;

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
        services.AddTransient<IIngredientGroupEditService, IngredientGroupEditService>();
        services.AddTransient<IShoppingListService, ShoppingListService>();
        services.AddTransient<IRecipesFinder, RecipesFinder>();

        return services;
    }
}