using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ReactiveUI;
using Recipes.Application.Services.Preferences;
using Recipes.Application.Services.RecipePicker;
using Recipes.Domain.Interfaces;
using Recipes.Presentation.Interfaces;

namespace Recipes.Presentation.ViewModels
{
    public class MainViewModel : ViewModelBase, IViewContainer
    {
        private readonly ILogger<MainViewModel> _logger;
        private ViewModelBase _content;
    
        public ViewModelBase Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        public MainViewModel() : this(NullLogger<MainViewModel>.Instance)
        { }

        public MainViewModel(ILogger<MainViewModel> logger)
        {
            _logger = logger;
            logger.LogInformation("ViewModelBase created");
        }
    }
}