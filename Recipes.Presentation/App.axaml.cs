using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Infrastructure;
using Recipes.Presentation.DataTypes;
using Recipes.Presentation.Interfaces;
using Recipes.Presentation.ViewModels;
using Recipes.Presentation.Views;
using System;
using System.Collections.Generic;

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
        private readonly MainMenuItemsBuilder _mainMenuItemsBuilder;

        public ApplicationViewInitializer(MainMenuItemsBuilder mainMenuItemsBuilder,
            MainWindow mainWindow, MainView mainView)
        {
            _mainWindow = mainWindow;
            _mainView = mainView;
            _mainMenuItemsBuilder = mainMenuItemsBuilder;
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

        public List<MainMenuItem> GetMainMenuItems()
        {
            return _mainMenuItemsBuilder.Build();
        }
    }

    internal class MainMenuItemsBuilder
    {
        private readonly ProductSearchViewModel _productSearchViewModel;
        private readonly RecipeSearchViewModel _recipeSearchViewModel;
        private readonly RecipeEditorViewModel _recipeEditorViewModel;

        public MainMenuItemsBuilder(ProductSearchViewModel productSearchViewModel,
            RecipeSearchViewModel recipeSearchViewModel,
            RecipeEditorViewModel recipeEditorViewModel)
        {
            _productSearchViewModel = productSearchViewModel;
            _recipeSearchViewModel = recipeSearchViewModel;
            _recipeEditorViewModel = recipeEditorViewModel;
        }

        public List<MainMenuItem> Build()
        {
            return new List<MainMenuItem>
                {
                    new()
                    {
                        Title = "Search by ingregients",
                        PageFactory = ()=>_productSearchViewModel
                    },
                    new()
                    {
                        Title = "Search by name",
                        PageFactory = ()=>_recipeSearchViewModel
                    },
                    new()
                    {
                        Title = "Add own recipe",
                        PageFactory = ()=>_recipeEditorViewModel
                    }
                };
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
                .AddTransient<MainMenuItemsBuilder>()
                .AddSingleton<MainWindow>()
                .AddSingleton<MainView>()
                .AddSingleton<ApplicationViewInitializer>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            AvaloniaXamlLoader.Load(this);
            Resources.Add(typeof(IServiceProvider), serviceProvider);
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