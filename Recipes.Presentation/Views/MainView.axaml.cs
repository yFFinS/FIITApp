using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Presentation.Interfaces;
using Recipes.Presentation.ViewModels;

namespace Recipes.Presentation.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            // DataContext = new MainViewModel();
            InitializeComponent();
        }
    }
}