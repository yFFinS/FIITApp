using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Infrastructure;
using Recipes.Presentation.ViewModels;
using Recipes.Presentation.Views;
using System;
using System.Collections.Generic;
using ReactiveUI;
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

            services.AddSingleton<IViewContainer>(x => x.GetRequiredService<IMainViewModel>());
            services.AddSingleton<IExceptionContainer>(x => x.GetRequiredService<IMainViewModel>());
            services.AddSingleton<IMainViewModel, MainViewModel>();
            services.AddSingleton<ProductSearchViewModel>();
            services.AddSingleton<RecipeSearchViewModel>();
            services.AddSingleton<RecipeViewFactory>();
            services.AddTransient<RecipeEditorViewModel>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton(x => new MainView
            {
                DataContext = x.GetRequiredService<IViewContainer>()
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            AvaloniaXamlLoader.Load(this);
            // RxApp.DefaultExceptionHandler = new UiExceptionHandler();
            Resources.Add(typeof(IServiceProvider), serviceProvider);
        }

        private void ConfigureMenu(IServiceCollection services)
        {
            services.AddSingleton(x => new List<MainMenuItem>
            {
                new()
                {
                    Title = "Искать по ингредиентам",
                    Page = x.GetRequiredService<ProductSearchViewModel>
                },
                new()
                {
                    Title = "Искать по имени",
                    Page = x.GetRequiredService<RecipeSearchViewModel>
                },
                new()
                {
                    Title = "Добавить свой рецепт",
                    Page = x.GetRequiredService<RecipeEditorViewModel>
                }
            });
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var provider = this.GetServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = provider.GetRequiredService<MainWindow>();
                var window = (MainWindow)desktop.MainWindow;
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