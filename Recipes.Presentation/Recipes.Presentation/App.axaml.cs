using System;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Recipes.Presentation.ViewModels;
using Recipes.Presentation.Views;

namespace Recipes.Presentation
{
    internal static class DependencyInjection
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddLogging(builder => builder.AddConsole());

            services.AddSingleton<ILogger>(x =>
                x.GetRequiredService<ILoggerFactory>().CreateLogger("Default"));

            services.AddTransient<MainViewModel>();
            services.AddSingleton(x => new MainWindow()
            {
                DataContext = x.GetRequiredService<MainViewModel>()
            });
            services.AddSingleton(x => new MainView()
            {
                DataContext = x.GetRequiredService<MainViewModel>()
            });
        }

        public static IServiceProvider GetServiceProvider(this IResourceHost control)
        {
            var serviceProvider = control.FindResource(typeof(IServiceProvider)) as IServiceProvider;
            return serviceProvider ?? throw new InvalidOperationException();
        }
    }

    public partial class App : Avalonia.Application
    {
        public override void Initialize()
        {
            IServiceCollection services = new ServiceCollection();
            services.ConfigureServices();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            AvaloniaXamlLoader.Load(this);
            Resources.Add(typeof(IServiceProvider), serviceProvider);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var provider = this.GetServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = provider.GetRequiredService<MainWindow>();
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = provider.GetRequiredService<MainView>();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}