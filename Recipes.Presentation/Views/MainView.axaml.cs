using System.Reactive.Concurrency;
using Avalonia.Controls;
using Avalonia.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Presentation.Interfaces;

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