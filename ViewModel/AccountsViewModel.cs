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
        private readonly ObservableCollection<EncryptableFieldModel> _fields;
        private string _searchBarText = string.Empty;
        private AccountModel _selectedAccount = null;
        private EncryptableFieldModel _selectedField = null;

        private IAccountRepository _accountRepository;
        private ICustomFieldRepository _customFieldRepository;
        private IUserIdentityStore _identity;
        private ILogger _logger;
        private ICryptographyService _cryptographyService;

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
                LoadFields();
                OnPropertyChanged();
            }
        }

        public EncryptableFieldModel SelectedField
        {
            get { return _selectedField; }
            set
            {
                _selectedField = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteAccountCommand { get; set; }
        public ICommand DeleteFieldCommand { get; set; }
        public ICommand AddAccountCommand { get; set; }
        public ICommand AddFieldCommand { get; set; }
        public ICommand EditAccountCommand { get; set; }
        public ICommand EditFieldCommand { get; set; }

        public ICollectionView AccountsCollectionView { get; }
        public ICollectionView FieldsCollectionView { get; }

        public AccountsViewModel(IUserIdentityStore identity, IAccountRepository accountRepository, 
            ICustomFieldRepository customFieldRepository, IDialogService dialogService, ILogger logger,
            ICryptographyService cryptographyService, ISynchronizationService synchronizationService,
            IAccountRestApi accountRestApi, ICustomFieldRestApi customFieldRestApi)
        {
            _identity = identity;
            _accountRepository = accountRepository;
            _customFieldRepository = customFieldRepository;
            _identity = identity;
            _logger = logger;
            _cryptographyService = cryptographyService;

            _accounts = new ObservableCollection<AccountModel>();
            _fields = new ObservableCollection<EncryptableFieldModel>();

            DeleteAccountCommand = new DeleteAccountCommand(this, accountRestApi, synchronizationService,
                identity, logger);
            DeleteFieldCommand = new DeleteFieldCommand(this, customFieldRestApi, synchronizationService, 
                identity, logger);
            AddAccountCommand = new AddNewAccountCommand(this, dialogService, logger, accountRestApi, 
                identity, cryptographyService, synchronizationService);
            AddFieldCommand = new AddFieldCommand(this, logger, dialogService, 
                identity, customFieldRestApi, cryptographyService, synchronizationService);
            EditAccountCommand = new EditAccountCommand(this, dialogService, logger, accountRestApi,
                identity, cryptographyService, synchronizationService);
            EditFieldCommand = new EditFieldCommand(this, logger, dialogService,
                identity, customFieldRestApi, cryptographyService, synchronizationService);

            AccountsCollectionView = CollectionViewSource.GetDefaultView(_accounts);
            AccountsCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(AccountModel.ServiceName)));
            AccountsCollectionView.Filter = FilterWithSearchBar;

            FieldsCollectionView = CollectionViewSource.GetDefaultView(_fields);

            LoadAccounts();
        }

        public void LoadAccounts()
        {
            IEnumerable<AccountModel> accounts = _accountRepository.GetByUserID(_identity.CurrentUser.ID);

            _accounts.Clear();

            foreach (AccountModel account in accounts)
            {
                _cryptographyService.Decrypt(account);
                _accounts.Add(account);
            }

            _logger.Information("Accounts list loaded.");
        }

        public void LoadFields()
        {
            if (SelectedAccount == null)
            {
                return;
            }

            IEnumerable<EncryptableFieldModel> customFields = _customFieldRepository.GetByAccountID(_selectedAccount.ID);

            _fields.Clear();

            EncryptableFieldModel passwordField = new EncryptableFieldModel();
            passwordField.ID = _selectedAccount.ID;
            passwordField.FieldName = "Password";   
            passwordField.Value = _selectedAccount.Password;
            passwordField.AccountId = _selectedAccount.ID;
            passwordField.IsDeletable = false;
            _fields.Add(passwordField);

            foreach (EncryptableFieldModel field in customFields)
            {
                _cryptographyService.Decrypt(field);
                _fields.Add(field);
            }

            _logger.Information("Custom fields list loaded.");
        }

        private bool FilterWithSearchBar(object obj)
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
