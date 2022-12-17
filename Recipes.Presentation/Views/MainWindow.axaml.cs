using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Recipes.Presentation.Views
{
    public partial class MainWindow : Window
    {
        private Panel MainPanel;
        internal Border ViewBorder;

        public Geometry CloseButtonGeometry;
        public MainWindow()
        {
            InitializeComponent();
            MainPanel = this.FindControl<Panel>("Main")!;
            ViewBorder = this.FindControl<Border>("View")!;
            CloseButtonGeometry =
                StreamGeometry.Parse("M 7,7 17.5,17.5 28,7 17.5,17.5 28,28 17.5,17.5 7,28 17.5,17.5 Z");
        }

        protected override void HandleWindowStateChanged(WindowState state)
        {
            base.HandleWindowStateChanged(state);
            MainPanel.Margin = WindowState == WindowState.Maximized ? new Thickness(8) : new Thickness(0);
        }

        private void CloseBtn_OnClick(object? sender, RoutedEventArgs e) => Close();

        private void MaximizeBtn_OnClick(object? sender, RoutedEventArgs e) => WindowState =
            WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

        private void MinimizeBtn_OnClick(object? sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    }
}