using Avalonia.Controls;
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