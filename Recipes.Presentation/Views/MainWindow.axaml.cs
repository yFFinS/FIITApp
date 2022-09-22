using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Recipes.Presentation.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseBtn_OnClick(object? sender, RoutedEventArgs e) => Close();
    }
}