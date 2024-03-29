﻿using Serilog;
using SnailPass.Model;
using SnailPass.Model.Interfaces;
using SnailPass.Services;
using SnailPass.ViewModel.Commands;
using SnailPass.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SnailPass.ViewModel
{
    public class AccountsViewModel : ViewModelBase, IRefreshable
    {
        private readonly ObservableCollection<AccountModel> _accounts;
        private readonly ObservableCollection<EncryptableFieldModel> _fields;
        private string _searchBarText = string.Empty;
        private AccountModel _selectedAccount = null;
        private EncryptableFieldModel _selectedField = null;
        private bool _isNetworkFunctionsEnabled;
        private string? _updationTime;
        private string? _creationTime;

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

                if (_selectedAccount != null)
                {
                    UpdationTime = DateTime.Parse(_selectedAccount.UpdateTime).ToShortDateString();
                    CreationTime = DateTime.Parse(_selectedAccount.CreationTime).ToShortDateString();
                }

                LoadFieldsAsync();
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

        public bool IsNetworkFunctionsEnabled
        {
            get { return _isNetworkFunctionsEnabled; }
            set
            {
                _isNetworkFunctionsEnabled = value;
                OnPropertyChanged();
            }
        }

        public string? UpdationTime
        {
            get { return _updationTime; }
            set
            {
                _updationTime = value;
                OnPropertyChanged();
            }
        }

        public string? CreationTime
        {
            get { return _creationTime; }
            set
            {
                _creationTime = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteAccountCommand { get; set; }
        public ICommand DeleteFieldCommand { get; set; }
        public ICommand AddAccountCommand { get; set; }
        public ICommand AddFieldCommand { get; set; }
        public ICommand EditAccountCommand { get; set; }
        public ICommand EditFieldCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public ICollectionView AccountsCollectionView { get; }
        public ICollectionView FieldsCollectionView { get; }

        public AccountsViewModel(IUserIdentityStore identity, IAccountRepository accountRepository, 
            ICustomFieldRepository customFieldRepository, IDialogService dialogService, ILogger logger,
            ICryptographyService cryptographyService, ISynchronizationService synchronizationService,
            IAccountRestApi accountRestApi, ICustomFieldRestApi customFieldRestApi, IApplicationModeStore applicationModeStore)
        {
            _identity = identity;
            _accountRepository = accountRepository;
            _customFieldRepository = customFieldRepository;
            _identity = identity;
            _logger = logger;
            _cryptographyService = cryptographyService;

            _accounts = new ObservableCollection<AccountModel>();
            _fields = new ObservableCollection<EncryptableFieldModel>();
            _isNetworkFunctionsEnabled = !applicationModeStore.IsLocalMode;

            DeleteAccountCommand = new DeleteAccountCommand(this, accountRestApi, synchronizationService,
                identity, logger);
            DeleteFieldCommand = new DeleteFieldCommand(this, customFieldRestApi, synchronizationService, 
                identity, logger);
            AddAccountCommand = new AddAccountCommand(this, dialogService, logger, accountRestApi, 
                identity, cryptographyService, synchronizationService);
            AddFieldCommand = new AddFieldCommand(this, logger, dialogService, 
                identity, customFieldRestApi, cryptographyService, synchronizationService);
            EditAccountCommand = new EditAccountCommand(this, dialogService, logger, accountRestApi,
                identity, cryptographyService, synchronizationService);
            EditFieldCommand = new EditFieldCommand(this, logger, dialogService,
                identity, customFieldRestApi, cryptographyService, synchronizationService);
            RefreshCommand = new RefreshCommand(this, logger, identity, synchronizationService);

            AccountsCollectionView = CollectionViewSource.GetDefaultView(_accounts);
            AccountsCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(AccountModel.ServiceName)));
            AccountsCollectionView.Filter = FilterWithSearchBar;

            FieldsCollectionView = CollectionViewSource.GetDefaultView(_fields);

            applicationModeStore.LocalModeDisabled += LocalModeDisabledHandler;
            applicationModeStore.LocalModeEnabled += LocalModeEnabledHandler;
            
            LoadAccountsAsync();
        }

        public async Task LoadAccountsAsync()
        {
            IEnumerable<AccountModel> accounts = 
                _accountRepository.GetByUserId(_identity.CurrentUser.ID);

            List<Task> tasks = new();
            foreach (AccountModel account in accounts)
            {
                tasks.Add(_cryptographyService.DecryptAsync(account));
            }
            await Task.WhenAll(tasks);

            _accounts.Clear();
            foreach (AccountModel account in accounts)
            {
                _accounts.Add(account);
            }

            _logger.Information("Accounts list loaded.");
        }

        public async Task LoadFieldsAsync()
        {
            if (SelectedAccount == null)
            {
                return;
            }

            IEnumerable<EncryptableFieldModel> customFields = 
                _customFieldRepository.GetByAccountID(_selectedAccount.ID);

            _fields.Clear();

            EncryptableFieldModel passwordField = new EncryptableFieldModel();
            passwordField.ID = _selectedAccount.ID;
            passwordField.FieldName = "Password";   
            passwordField.Value = _selectedAccount.Password;
            passwordField.AccountId = _selectedAccount.ID;
            passwordField.IsDeletable = false;
            _fields.Add(passwordField);

            List<Task> tasks = new();
            foreach (EncryptableFieldModel field in customFields)
            {
                tasks.Add(_cryptographyService.DecryptAsync(field));
            }
            await Task.WhenAll(tasks);

            foreach (EncryptableFieldModel field in customFields)
            {
                _fields.Add(field);
            }

            _logger.Information("Fields list loaded.");
        }

        private bool FilterWithSearchBar(object obj)
        {
            if (obj is AccountModel account)
            {
                string serviceName = account.ServiceName.ToLower();
                string login = account.Login.ToLower();
                string searchBarText = SearchBarText.ToLower();

                return serviceName.Contains(searchBarText) ||
                    login.Contains(searchBarText);
            }
            return false;
        }

        private void LocalModeEnabledHandler(object? s, EventArgs args)
        {
            IsNetworkFunctionsEnabled = false;
        }

        private void LocalModeDisabledHandler(object? s, EventArgs args)
        {
            IsNetworkFunctionsEnabled = true;
        }

        public async Task RefreshAsync(object? args)
        {
            await LoadAccountsAsync();
        }
    }
}