using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Commands;
using SnailPass_Desktop.ViewModel.Services;
using SnailPass_Desktop.ViewModel.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    public class AccountsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<AccountModel> _accounts;
        private readonly ObservableCollection<CustomFieldModel> _customFields;
        private string _searchBarText = string.Empty;
        private AccountModel _selectedAccount = null;

        private IAccountRepository _accountRepository;
        private ICustomFieldRepository _customFieldRepository;
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
                AccountsCollectionView.Refresh();
            }
        }

        public AccountModel SelectedAccount
        {
            get { return _selectedAccount; }
            set 
            {
                _selectedAccount = value;
                OnPropertyChanged();
                LoadCustomFields();
            }
        }

        public ICommand RemoveCommand { get; set; }
        public ICommand AddNewCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand AddCustomFieldCommand { get; set; }
        public ICommand DecryptCommand { get; set; }

        public ICollectionView AccountsCollectionView { get; }
        public ICollectionView CustomFieldCollectionView { get; }

        public AccountsViewModel(IUserIdentityStore identity, IAccountRepository accountRepository, 
            ICustomFieldRepository customFieldRepository, ISynchronizationService synchronizationService, 
            IDialogService dialogService, ILogger logger, IRestClient httpClient, ISymmetricCryptographer cryptographer,
            IMasterPasswordEncryptor encryptor)
        {
            _identity = identity;
            _accountRepository = accountRepository;
            _customFieldRepository = customFieldRepository;
            _synchronizationService = synchronizationService;
            _logger = logger;
            _identity = identity;

            _accounts = new ObservableCollection<AccountModel>();
            _customFields = new ObservableCollection<CustomFieldModel>();

            RemoveCommand = new RemoveCommand();
            AddNewCommand = new AddNewAccountCommand(this, dialogService, logger, httpClient, identity);
            UpdateCommand = new UpdateCommand();
            AddCustomFieldCommand = new AddCustomFieldCommand(this, logger, dialogService, identity, httpClient);
            DecryptCommand = new DecryptPasswordCommand(this, cryptographer, encryptor, logger, identity);

            AccountsCollectionView = CollectionViewSource.GetDefaultView(_accounts);
            AccountsCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(AccountModel.ServiceName)));
            AccountsCollectionView.Filter = FilerWithSearchBar;

            CustomFieldCollectionView = CollectionViewSource.GetDefaultView(_customFields);

            LoadAccountsAsync();
        }

        public async void LoadAccountsAsync()
        {
            await _synchronizationService.SynchronizeAsync(_identity.CurrentUser.Email);

            IEnumerable<AccountModel> accounts = _accountRepository.GetByUserID(_identity.CurrentUser.ID);
            _accounts.Clear();
            foreach (AccountModel account in accounts)
            {
                _accounts.Add(account);
            }

            _logger.Information("Accounts list loaded.");
        }

        public void LoadCustomFields()
        {
            IEnumerable<CustomFieldModel> customFields = _customFieldRepository.GetByAccountID(_identity.CurrentUser.ID);
            _customFields.Clear();
            foreach (CustomFieldModel field in customFields)
            {
                _customFields.Add(field);
            }

            _logger.Information("Custom fields list loaded.");
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
