using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ReactiveUI;
using Recipes.Application.Services.Preferences;
using Recipes.Application.Services.RecipePicker;
using Recipes.Domain.Interfaces;

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
            var repo = new RecipeRepository();
            var picker = new RecipePicker(NullLogger<RecipePicker>.Instance, repo, new PreferenceService(NullLogger<PreferenceService>.Instance, ""));
            Content = new SearchViewModel(picker, vm => Content = vm);
        }

        public MainViewModel(ILogger<MainViewModel> logger)
        {
            _logger = logger;
            logger.LogInformation("ViewModelBase created");
        }
    }
}