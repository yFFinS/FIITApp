using System.Collections.Generic;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ReactiveUI;
using Recipes.Application.Services.Preferences;
using Recipes.Application.Services.RecipePicker;
using Recipes.Domain.Interfaces;
using Recipes.Presentation.DataTypes;
using Recipes.Presentation.Interfaces;

namespace Recipes.Presentation.ViewModels
{
    public class MainViewModel : ViewModelBase, IViewContainer
    {
        private readonly ILogger<MainViewModel> _logger;
        private ViewModelBase _content;
        private readonly List<MainMenuItem> _menuItems;
        private MainMenuItem _selectedView;

        public List<MainMenuItem> MenuItems => _menuItems;
        public ReactiveCommand<ViewModelBase, Unit> ChangeView { get; }

        public ViewModelBase Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        public MainMenuItem SelectedView
        {
            get => _selectedView;
            set => this.RaiseAndSetIfChanged(ref _selectedView, value);
        }

        public MainViewModel(List<MainMenuItem> menuItems) : this(NullLogger<MainViewModel>.Instance, menuItems)
        { }

        public MainViewModel(ILogger<MainViewModel> logger, List<MainMenuItem> menuItems)
        {
            _menuItems = menuItems;
            ChangeView = ReactiveCommand.Create<ViewModelBase>(view =>
            {
                Content = view;
            });

            Content = menuItems[0].Page;
            _logger = logger;
            logger.LogInformation("ViewModelBase created");
        }
    }
}