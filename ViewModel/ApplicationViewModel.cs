using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.ViewModel.Commands;
using SnailPass_Desktop.ViewModel.Services;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    internal class ApplicationViewModel : ViewModelBase
    {
        private INavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand NavigateHomeCommand { get; set; }
        public ICommand NavigateAccountsCommand { get; set; }
        public ICommand NavigateNotesCommand { get; set; }

        public ApplicationViewModel(INavigationStore navigationStore, IUserIdentityStore identityStore,
            IAccountRepository accountRepository, IDialogService dialogService, ILogger logger)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChange += OnCurrentViewModelChange;

            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navigationStore, 
                () => new HomeViewModel(identityStore, navigationStore, logger), logger);
            NavigateAccountsCommand = new NavigateCommand<AccountsViewModel>(navigationStore, 
                () => new AccountsViewModel(identityStore, accountRepository, dialogService, logger), logger);
            NavigateNotesCommand = new NavigateCommand<NotesViewModel>(navigationStore, 
                () => new NotesViewModel(identityStore, navigationStore, logger), logger);

            NavigateAccountsCommand.Execute(null);
        }

        private void OnCurrentViewModelChange()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}