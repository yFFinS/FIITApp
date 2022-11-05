using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Infrastructure;
using Recipes.Presentation.ViewModels;
using Recipes.Presentation.Views;
using System;
using Recipes.Presentation.Interfaces;

namespace Recipes.Presentation
{
    internal static class DependencyInjection
    {
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
            var services = Bootstrap.ConfigureServices();

            services.AddSingleton<IViewContainer, MainViewModel>(x => new MainViewModel()
            {
                Content = x.GetRequiredService<SelectionViewModel>()
            });
            services.AddSingleton<SelectionViewModel>();
            services.AddSingleton<SearchViewModel>();
            services.AddSingleton(x => new MainWindow());
            services.AddSingleton(x => new MainView()
            {
                DataContext = x.GetRequiredService<MainViewModel>()
            });

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