﻿using Serilog;
using SnailPass.Model;
using SnailPass.Model.Interfaces;
using SnailPass.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnailPass.ViewModel.Commands
{
    public class DeleteAccountCommand : CommandBase
    {
        private AccountsViewModel _viewModel;
        private IAccountRestApi _accountRestApi;
        private ISynchronizationService _synchronizationService;
        private IUserIdentityStore _identity;
        private ILogger _logger;

        public DeleteAccountCommand(AccountsViewModel viewModel, IAccountRestApi accountRestApi,
            ISynchronizationService synchronizationService, IUserIdentityStore identity,
            ILogger logger)
        {
            _viewModel = viewModel;
            _accountRestApi = accountRestApi;
            _synchronizationService = synchronizationService;
            _identity = identity;
            _logger = logger;
        }

        public override bool CanExecute(object? parameters)
        {
            return _viewModel.SelectedAccount != null;
        }

        public async override void Execute(object? parameter)
        {
            AccountModel account = _viewModel.SelectedAccount;

            _logger.Information($"Execute deletion for account with ID: \"{account.ID}\".");

            HttpStatusCode? code = await _accountRestApi.DeleteAccountAsync(account.ID);
            if (code == HttpStatusCode.OK)
            {
                await _synchronizationService.SynchronizeAsync(_identity.CurrentUser.Email);
                await _viewModel.LoadAccountsAsync();
            }
        }
    }
}
