using Serilog;
using SnailPass.Services;
using SnailPass.Services;
using SnailPass.ViewModel.Factories;
using SnailPass.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnailPass.ViewModel
{
    public class StartupViewModel : ViewModelBase
    {
        private readonly INavigationStore _navigationStore;
        private readonly ILogger _logger;
        private readonly IDialogService _dialogService;

        private bool _isViewVisible = true;
        private bool _isDialogOpened = false;

        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsDialogOpened
        {
            get { return _isDialogOpened; }
            set
            {
                _isDialogOpened = value;
                OnPropertyChanged();
            }
        }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        public string HeaderName => _navigationStore.TextHeader;

        public StartupViewModel(INavigationStore navigationStore, ILogger logger, IViewModelFactory viewModelFactory,
            IUpdationService updationService, IDialogService dialogService)
        {
            navigationStore.CurrentViewModel = viewModelFactory.Create(typeof(LoginViewModel));

            _navigationStore = navigationStore;
            _logger = logger;
            _dialogService = dialogService;

            _navigationStore.CurrentViewModelChange += OnCurrentViewModelChange;
            _navigationStore.TextHeaderChange += OnTextHeaderChange;
            _navigationStore.CurrentViewModel.PropertyChanged += OnPropertyChangedVisibilityHandler;
            _dialogService.DialogOpened +=DialogOpenedHandler;
            _dialogService.DialogClosed += DialogClosedHandler;

            updationService.StartAsync();
        }

        private void OnPropertyChangedVisibilityHandler(object? s, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == "IsViewVisible")
            {
                IsViewVisible = false;
            }
        }

        private void DialogOpenedHandler(object? sender, EventArgs args)
        {
            IsDialogOpened = true;
        }

        private void DialogClosedHandler(object? sender, EventArgs args)
        {
            if (_dialogService.IsAllDialogsClosed)
            {
                IsDialogOpened = false;
            }
        }

        private void OnTextHeaderChange()
        {
            OnPropertyChanged(nameof(HeaderName));
        }

        private void OnCurrentViewModelChange()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
            _navigationStore.CurrentViewModel.PropertyChanged += OnPropertyChangedVisibilityHandler;
        }
    }
}
