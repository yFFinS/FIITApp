using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Infrastructure;
using Recipes.Infrastructure.DataBase;
using Recipes.Presentation.DataTypes;
using Recipes.Presentation.Interfaces;
using Recipes.Presentation.ViewModels;
using Recipes.Presentation.Views;
using System;

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


    internal class ApplicationViewInitializer
    {
        private readonly MainWindow _mainWindow;
        private readonly MainView _mainView;

        public ApplicationViewInitializer(MainWindow mainWindow, MainView mainView)
        {
            _mainWindow = mainWindow;
            _mainView = mainView;
        }

        public void InitializeLifetime(IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = _mainWindow;
            _mainWindow.ViewBorder.Child = _mainView;
        }

        public void InitializeLifetime(ISingleViewApplicationLifetime singleView)
        {
            singleView.MainView = _mainView;
        }
    }

    public partial class App : Avalonia.Application
    {
        public override void Initialize()
        {
            var services = Bootstrap.ConfigureServices();

            ConfigureMenu(services);

            services
                .AddSingleton<IViewContainer>(x => x.GetRequiredService<IMainViewModel>())
                .AddSingleton<IExceptionContainer>(x => x.GetRequiredService<IMainViewModel>())
                .AddSingleton<IMainViewModel, MainViewModel>()
                .AddSingleton<ProductSearchViewModel>().AddFactory<ProductSearchViewModel>()
                .AddSingleton<RecipeSearchViewModel>().AddFactory<RecipeSearchViewModel>()
                .AddTransient<RecipeEditorViewModel>().AddFactory<RecipeEditorViewModel>()
                .AddSingleton<RecipeViewFactory>()
                .AddSingleton<MainWindow>()
                .AddSingleton<MainView>()
                .AddSingleton<ApplicationViewInitializer>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            InitDatabases(serviceProvider);

            AvaloniaXamlLoader.Load(this);
            Resources.Add(typeof(IServiceProvider), serviceProvider);
        }

        private static void InitDatabases(IServiceProvider serviceProvider)
        {
            var ftpServices = serviceProvider.GetRequiredService<FtpServices>();
            ftpServices.DownloadUpdatesIfPresent();

            var database = serviceProvider.GetRequiredService<IDataBase>();
            database.GetAllProducts();
            database.GetAllRecipes();
        }

        private void ConfigureMenu(IServiceCollection services)
        {
            services
                .AddSingleton<MainMenuItem, ProductSearchMenuItem>()
                .AddSingleton<MainMenuItem, RecipeSearchMenuItem>()
                .AddSingleton<MainMenuItem, RecipeEditorMenuItem>();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var provider = this.GetServiceProvider();
            var initializer = provider.GetRequiredService<ApplicationViewInitializer>();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                initializer.InitializeLifetime(desktop);
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                initializer.InitializeLifetime(singleViewPlatform);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}