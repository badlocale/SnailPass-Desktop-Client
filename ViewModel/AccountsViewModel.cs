using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Commands;
using SnailPass_Desktop.ViewModel.Services;
using SnailPass_Desktop.ViewModel.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Data;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    public class AccountsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<AccountModel> _accounts;
        private string _searchBarText = string.Empty;
        private string _customFieldName;
        private string _customFieldValue;
        private IAccountRepository _repository;
        private ISynchronizationService _synchronizationService;
        private IUserIdentityStore _identity;
        private ILogger _logger;

        public string SearchBarText
        {
            get { return _searchBarText; }
            set
            {
                _searchBarText = value;
                OnPropertyChanged();
                AccountsCollectiionView.Refresh();
            }
        }

        public string CustomFieldName
        {
            get { return _customFieldName; }
            set
            {
                _customFieldName = value;
                OnPropertyChanged();
            }
        }

        public string CustomFieldValue
        {
            get { return _customFieldValue; }
            set
            {
                _customFieldValue = value;
                OnPropertyChanged();
            }
        }

        public ICommand RemoveCommand { get; set; }
        public ICommand AddNewCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        public ICollectionView AccountsCollectiionView { get; }

        public AccountsViewModel(IUserIdentityStore identity, IAccountRepository accountRepository, 
            ISynchronizationService synchronizationService, IDialogService dialogService, ILogger logger, 
            IRestClient httpClient)
        {
            _identity = identity;
            _repository = accountRepository;
            _synchronizationService = synchronizationService;
            _logger = logger;
            _identity = identity;

            _accounts = new ObservableCollection<AccountModel>();

            RemoveCommand = new RemoveCommand();
            AddNewCommand = new AddNewAccountCommand(this, dialogService, logger, httpClient, identity);
            UpdateCommand = new UpdateCommand();

            AccountsCollectiionView = CollectionViewSource.GetDefaultView(_accounts);
            AccountsCollectiionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(AccountModel.ServiceName)));
            AccountsCollectiionView.Filter = FilerWithSearchBar;

            LoadAccountListAsync();
        }

        public async void LoadAccountListAsync()
        {
            await _synchronizationService.SynchronizeAsync(_identity.CurrentUser.Email);
            IEnumerable<AccountModel> accounts = _repository.GetByUserID(_identity.CurrentUser.ID);
            _accounts.Clear();
            foreach (AccountModel account in accounts)
            {
                _accounts.Add(account);
            }

            _logger.Information("Accounts list loaded.");
        }

        private bool FilerWithSearchBar(object obj)
        {
            if (obj is AccountModel account)
            {
                return account.ServiceName.Contains(SearchBarText) ||
                    account.Login.Contains(SearchBarText);
            }
            return false;
        }
    }
}
