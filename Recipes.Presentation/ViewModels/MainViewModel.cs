using Microsoft.Extensions.Logging;

namespace Recipes.Presentation.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ILogger<MainViewModel> _logger;

        public MainViewModel(ILogger<MainViewModel> logger)
        {
            _logger = logger;
            logger.LogInformation("ViewModelBase created");
        }

        public string Greeting => "Welcome to Avalonia!";
    }
}