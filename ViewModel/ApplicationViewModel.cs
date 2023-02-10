using Serilog;
using SnailPass_Desktop.Data.API;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.Services;
using SnailPass_Desktop.ViewModel.Commands;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    public class ApplicationViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        private bool _isDialogOpened = false;

        private INavigationStore _navigationStore;
        private IDialogService _dialogService;

        public bool IsDialogOpened
        {
            get { return _isDialogOpened; }
            set
            {
                _isDialogOpened = value;
                OnPropertyChanged();
            }
        }

        public ICommand NavigateHomeCommand { get; set; }
        public ICommand NavigateAccountsCommand { get; set; }
        public ICommand NavigateNotesCommand { get; set; }

        public ApplicationViewModel(INavigationStore navigationStore, IUserIdentityStore identity,
            IAccountRepository accountRepository, ICustomFieldRepository customFieldRepository, 
            IDialogService dialogService, ILogger logger, IAccountRestApi accountRestApi, 
            ICustomFieldRestApi customFieldRestApi, ISynchronizationService synchronizationService, 
            ICryptographyService cryptographyService, IApplicationModeStore applicationModeStore)
        {
            _navigationStore = navigationStore;
            _dialogService = dialogService;

            _navigationStore.CurrentViewModelChange += CurrentViewModelChangeHandler;
            RestApiBase.TokenExpired += TokenExpiredEventHandler;
            RestApiBase.ServerNotResponding += ServerNotRespondingHandler;

            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navigationStore, 
                () => new HomeViewModel(identity, navigationStore, logger), null);
            NavigateAccountsCommand = new NavigateCommand<AccountsViewModel>(navigationStore, 
                () => new AccountsViewModel(identity, accountRepository, customFieldRepository, 
                dialogService, logger, cryptographyService, synchronizationService, 
                accountRestApi, customFieldRestApi, applicationModeStore), null);
            NavigateNotesCommand = new NavigateCommand<NotesViewModel>(navigationStore, 
                () => new NotesViewModel(identity, navigationStore, logger), null);

            NavigateAccountsCommand.Execute(null);
        }

        private void CurrentViewModelChangeHandler()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void TokenExpiredEventHandler(object? sender, EventArgs args)
        {
            IsDialogOpened = true;
            //_dialogService.ShowDialog<>();
            IsDialogOpened = false;
        }

        private void ServerNotRespondingHandler(object? sender, EventArgs args)
        {
            IsDialogOpened = true;
            //_dialogService.ShowDialog("");
            IsDialogOpened = false;
        }
    }
}