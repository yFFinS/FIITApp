using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace Recipes.Presentation.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
#if DEBUG
        public MainViewModel() { }
#endif
        
        private readonly ILogger<MainViewModel> _logger;
        private ViewModelBase _content;
        private readonly List<MainMenuItem> _menuItems;
        private MainMenuItem _selectedView;
        private bool _hasErrors;
        private bool _menuOpened;

        public List<MainMenuItem> MenuItems => _menuItems;

        public ReactiveCommand<Func<ViewModelBase>, Unit> ChangeView { get; }

        public bool HasErrors   
        {
            get => _hasErrors;
            set => this.RaiseAndSetIfChanged(ref _hasErrors, value);
        }
        
        public ObservableCollection<Exception> Errors { get; private set; }
        
        public ReactiveCommand<Exception, Unit> RemoveExceptionCommand { get; }

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

        public MainViewModel(IEnumerable<MainMenuItem> menuItems) : this(NullLogger<MainViewModel>.Instance, menuItems)
        { }

        public MainViewModel(ILogger<MainViewModel> logger, IEnumerable<MainMenuItem> menuItems)
        {
            _menuItems = menuItems.ToList();
            ChangeView = ReactiveCommand.Create<Func<ViewModelBase>>(getView =>
            {
                Content = getView();
            });
            
            _logger = logger;
            logger.LogInformation("ViewModelBase created");

            Errors = new ObservableCollection<Exception>(Enumerable.Empty<Exception>());
            HasErrors = false;
            RemoveExceptionCommand = ReactiveCommand.Create<Exception>(RemoveException);
        }

        public void AddException(Exception ex)
        {
            Errors.Add(ex);
            HasErrors = true;
        }

        private void RemoveException(Exception ex)
        {
            Errors.Remove(ex);
            HasErrors = Errors.Any();
        }
    }
}