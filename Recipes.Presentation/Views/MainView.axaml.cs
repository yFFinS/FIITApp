using Avalonia.Controls;
using Recipes.Presentation.ViewModels;

namespace Recipes.Presentation.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
        }
    }
}