using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Recipes.Presentation.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ILogger<MainViewModel> _logger;

        public MainViewModel() : this(NullLogger<MainViewModel>.Instance)
        {
        }

        public MainViewModel(ILogger<MainViewModel> logger)
        {
            _logger = logger;
            logger.LogInformation("ViewModelBase created");
        }
    }
}