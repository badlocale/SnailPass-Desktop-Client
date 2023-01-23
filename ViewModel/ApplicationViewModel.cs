using Serilog;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Commands;
using SnailPass_Desktop.ViewModel.Services;
using SnailPass_Desktop.ViewModel.Stores;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    public class ApplicationViewModel : ViewModelBase
    {
        private INavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand NavigateHomeCommand { get; set; }
        public ICommand NavigateAccountsCommand { get; set; }
        public ICommand NavigateNotesCommand { get; set; }

        public ApplicationViewModel(INavigationStore navigationStore, IUserIdentityStore identity,
            IAccountRepository accountRepository, ICustomFieldRepository fieldRepository, 
            IDialogService dialogService, ILogger logger, IRestClient httpClient, 
            ISynchronizationService synchronizationService, ICryptographyService cryptographyService)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChange += OnCurrentViewModelChange;

            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navigationStore, 
                () => new HomeViewModel(identity, navigationStore, logger), null);
            NavigateAccountsCommand = new NavigateCommand<AccountsViewModel>(navigationStore, 
                () => new AccountsViewModel(identity, accountRepository, fieldRepository, dialogService, logger, 
                httpClient, cryptographyService, synchronizationService), null);
            NavigateNotesCommand = new NavigateCommand<NotesViewModel>(navigationStore, 
                () => new NotesViewModel(identity, navigationStore, logger), null);

            NavigateAccountsCommand.Execute(null);
        }

        private void OnCurrentViewModelChange()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}