using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Infrastructure;
using Recipes.Presentation.ViewModels;
using Recipes.Presentation.Views;
using System;
using System.Collections.Generic;
using Recipes.Application.Interfaces;
using Recipes.Domain.Interfaces;
using Recipes.Presentation.DataTypes;
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

            ConfigureMenu(services);

            services.AddSingleton<IViewContainer, MainViewModel>();
            services.AddSingleton(x => new Lazy<IViewContainer>(x.GetRequiredService<IViewContainer>));
            //todo
            services.AddSingleton<IRecipeRepository>(x => null);
            services.AddSingleton<IProductRepository>(x => null);
            services.AddSingleton<IImageLoader>(x => null);
            services.AddSingleton<ProductSearchViewModel>();
            services.AddSingleton<RecipeSearchViewModel>();
            services.AddSingleton(x => new MainWindow());
            services.AddSingleton(x => new MainView
            {
                DataContext = x.GetRequiredService<IViewContainer>()
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            AvaloniaXamlLoader.Load(this);
            Resources.Add(typeof(IServiceProvider), serviceProvider);
        }

        private void ConfigureMenu(IServiceCollection services)
        {
            services.AddSingleton(x => new List<MainMenuItem>
            {
                new()
                {
                    Title = "Search by ingregients",
                    Page = x.GetRequiredService<ProductSearchViewModel>()
                },
                new()
                {
                    Title = "Search by name",
                    Page = x.GetRequiredService<RecipeSearchViewModel>()
                }
            });
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var provider = this.GetServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = provider.GetRequiredService<MainWindow>();
                var window = desktop.MainWindow as MainWindow;
                window.ViewBorder.Child = provider.GetRequiredService<MainView>();
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = provider.GetRequiredService<MainView>();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}