using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ReactiveUI;

namespace Recipes.Presentation.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ILogger<MainViewModel> _logger;
        private ViewModelBase _content;
    
        public ViewModelBase Content
        {
            get => _content;
            private set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        public MainViewModel() : this(NullLogger<MainViewModel>.Instance)
        {
            Content = new SearchViewModel(new RecipesDataBase().Items, vm => Content = vm);
        }

        public MainViewModel(ILogger<MainViewModel> logger)
        {
            _logger = logger;
            logger.LogInformation("ViewModelBase created");
        }
    }
}