﻿using Avalonia;
using Avalonia.ReactiveUI;
using System;

namespace Recipes.Presentation.Desktop
{
    internal static class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            // AppDomain.CurrentDomain.UnhandledException += LogException;
            var exit = BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
            Environment.Exit(exit);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
        // Avalonia configuration, don't remove; also used by visual designer.
    }
}