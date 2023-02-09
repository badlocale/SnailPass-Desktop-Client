﻿using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.ViewModel.Stores;
using System.Net;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Services;
using System.Net.Http;
using SnailPass_Desktop.Model.Cryptography;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class LoginCommand : CommandBase
    {
        private LoginViewModel _viewModel;
        private IUserIdentityStore _identity;
        private IUserRepository _repository;
        private IKeyGenerator _encryptor;
        private IUserRestApi _userRestApi;
        private ILogger _logger;
        private IDialogService _dialogService;
        private ISynchronizationService _synchronizationService;
        private IApplicationModeStore _modeStore;

        public LoginCommand(LoginViewModel viewModel, IUserIdentityStore identity, IUserRestApi userRestApi,
            IUserRepository repository, IKeyGenerator encryptor, ILogger logger, IDialogService dialogService, 
            ISynchronizationService synchronizationService, IApplicationModeStore modeStore)
        {
            _viewModel = viewModel;
            _identity = identity;
            _userRestApi = userRestApi;
            _repository = repository;
            _encryptor = encryptor;
            _logger = logger;
            _dialogService = dialogService;
            _synchronizationService = synchronizationService;
            _modeStore = modeStore;
        }

        public override async void Execute(object? parameter)
        {
            _viewModel.ErrorMessage = string.Empty;
            _viewModel.ValidateEmail();
            _viewModel.ValidatePassword();

            if (_viewModel.HasErrors)
            {
                return;
            }

            UserModel? user = null;
            _identity.Master = _viewModel.Password;

            string apiKey = _encryptor.Encrypt(_identity.Master, _viewModel.Email, CryptoConstants.NETWORK_ITERATIONS_COUNT);

            _logger.Information($"Execute logging for E-mail: \"{_viewModel.Email}\" with hashed key \"{apiKey}\".");

            try
            {
                //Authorization with server

                HttpStatusCode? code = await _userRestApi.LoginAsync(_viewModel.Email, apiKey);

                if (code == HttpStatusCode.OK)
                {
                    await _synchronizationService.SynchronizeAsync(_viewModel.Email);
                    user = _repository.GetByEmail(_viewModel.Email);
                }
                else if (code == HttpStatusCode.Unauthorized)
                {
                    _viewModel.ErrorMessage = $"Not correct E-mail or password";
                }
                else
                {
                    _viewModel.ErrorMessage = $"Some error with code \"{code}\"";
                }
            }
            catch (HttpRequestException e)
            {
                //Local mode authorization

                _viewModel.ErrorMessage = $"Some trouble with server connection.";

                string localRepositoryKey = _encryptor.Encrypt(_viewModel.Password, _viewModel.Email, CryptoConstants.LOCAL_ITERATIONS_COUNT);

                bool isUserValid = _repository.AuthenticateLocally(localRepositoryKey, _viewModel.Email);
                if (isUserValid)
                {
                    _logger.Information($"User with E-mail {_viewModel.Email} is authenticated locally.");

                    bool? isLocalMode = _dialogService.ShowDialog("LocalModeDialog");
                    if (isLocalMode == true)
                    {
                        _modeStore.IsLocalMode = true;
                        user = _repository.GetByEmail(_viewModel.Email);
                    }
                }
            }

            if (user != null)
            {
                _identity.CurrentUser = user;
                _viewModel.IsViewVisible = false;
            }
        }
    }
}
