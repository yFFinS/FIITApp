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
        
        public MainView(IViewContainer viewContainer) : this()
        {
            DataContext = viewContainer;
        }
    }
}