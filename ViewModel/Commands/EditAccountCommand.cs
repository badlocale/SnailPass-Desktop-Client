﻿using Serilog;
using SnailPass.Model;
using SnailPass.Model.Interfaces;
using SnailPass.Services;
using SnailPass.ViewModel.Stores;
using System;
using System.Net;

namespace SnailPass.ViewModel.Commands
{
    public class EditAccountCommand : CommandBase
    {
        private AccountsViewModel _viewModel;
        private IDialogService _dialogService;
        private ILogger _logger;
        private IAccountRestApi _accountsRestApi;
        private IUserIdentityStore _identity;
        private ICryptographyService _cryptographyService;
        private ISynchronizationService _synchronizationService;

        public EditAccountCommand(AccountsViewModel viewModel, IDialogService dialogService,
            ILogger logger, IAccountRestApi accountRestApi, IUserIdentityStore identity,
            ICryptographyService cryptographyService, ISynchronizationService synchronizationService)
        {
            _viewModel = viewModel;
            _dialogService = dialogService;
            _logger = logger;
            _accountsRestApi = accountRestApi;
            _identity = identity;
            _cryptographyService = cryptographyService;
            _synchronizationService = synchronizationService;
        }

        public override bool CanExecute(object? parameters)
        {
            return _viewModel.SelectedAccount != null;
        }

        public async override void Execute(object? parameter)
        {
            EditAccountViewModel? dialogVM = _dialogService.ShowDialog<EditAccountViewModel>();

            if (dialogVM != null)
            {
                AccountModel newData = dialogVM.CreateModel();
                AccountModel selectedAccount = (AccountModel)_viewModel.SelectedAccount.Clone();

                _logger.Information($"Execute 'edit account data' for user: \"{_identity.CurrentUser.Email}\".");

                if (!string.IsNullOrEmpty(newData.ServiceName))
                {
                    selectedAccount.ServiceName = newData.ServiceName;
                }

                if (!string.IsNullOrEmpty(newData.Login))
                {
                    selectedAccount.Login = newData.Login;
                }

                if (!string.IsNullOrEmpty(newData.Password))
                {
                    selectedAccount.Password = newData.Password;
                }

                await _cryptographyService.EncryptAsync(selectedAccount);

                HttpStatusCode? code = await _accountsRestApi.PutAccountAsync(selectedAccount);
                if (code == HttpStatusCode.OK)
                {
                    await _synchronizationService.SynchronizeAsync(_identity.CurrentUser.Email);
                    await _viewModel.LoadAccountsAsync();
                }
            }
            else
            {
                _logger.Information("Dialog 'edit account data' cancelled.");
            }
        }
    }
}
