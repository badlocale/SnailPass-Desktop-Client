﻿using Serilog;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Diagnostics;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class RefreshCommand : CommandBase
    {
        private AccountsViewModel _viewModel;
        private ISynchronizationService _synchronizationService;
        private ILogger _logger;
        private IUserIdentityStore _identity;

        public RefreshCommand(AccountsViewModel viewModel, ILogger logger, IUserIdentityStore identity, 
            ISynchronizationService synchronizationService)
        {
            _viewModel = viewModel;
            _synchronizationService = synchronizationService;
            _logger = logger;
            _identity = identity;

        }

        public async override void Execute(object? parameter)
        {
            _logger.Information($"Execute refresh command for user: \"{_identity.CurrentUser.Email}\".");
            await _synchronizationService.SynchronizeAsync(_identity.CurrentUser.Email);
            _viewModel.LoadAccountsAsync();
        }
    }
}
