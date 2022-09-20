using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Recipes.Infrastructure;

public static class Bootstrap
{
    public static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddLogging(builder => builder.AddConsole());

        services.AddSingleton<ILogger>(x =>
            x.GetRequiredService<ILoggerFactory>().CreateLogger("Default"));

        return services;
    }
}