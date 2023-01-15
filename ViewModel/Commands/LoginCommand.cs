using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Cryptography;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Net;
using System.Net.Mail;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Services;
using System.Configuration;
using SnailPass_Desktop.Data;
using System.Threading;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly int NETWORK_ITERATION_COUNT;
        private readonly int LOCAL_ITERATION_COUNT;

        private LoginViewModel _viewModel;
        private IUserIdentityStore _identity;
        private IUserRepository _repository;
        private IMasterPasswordEncryptor _encryptor;
        private IRestClient _httpClient;
        private ILogger _logger;
        private IDialogService _dialogService;
        private ISynchronizationService _synchronizationService;

        public LoginCommand(LoginViewModel viewModel, IUserIdentityStore identity, IRestClient httpClient,
            IUserRepository repository, IMasterPasswordEncryptor encryptor, ILogger logger, IDialogService dialogService, 
            ISynchronizationService synchronizationService)
        {
            NETWORK_ITERATION_COUNT = int.Parse(ConfigurationManager.AppSettings["network_hash_iterations"]);
            LOCAL_ITERATION_COUNT = int.Parse(ConfigurationManager.AppSettings["local_hash_iterations"]);

            _viewModel = viewModel;
            _identity = identity;
            _httpClient = httpClient;
            _repository = repository;
            _encryptor = encryptor;
            _logger = logger;
            _dialogService = dialogService;
            _synchronizationService = synchronizationService;
        }

        public override bool CanExecute(object? parameter)
        {
            bool validData = false;

            if (IsEmailValid(_viewModel.Email) == true && _viewModel.Email.Length >= 5 &&
                _viewModel.Password != null && _viewModel.Password.Length >= 10)
            {
                validData = true;
            }

            return validData;
        }

        public override async void Execute(object? parameter)
        {
            UserModel? user = null;
            _viewModel.ErrorMessage = null;
            _identity.Master = _viewModel.Password;

            string apiKey = _encryptor.Encrypt(_viewModel.Password, _viewModel.Email, NETWORK_ITERATION_COUNT);

            _logger.Information($"Execute logging for E-mail: \"{_viewModel.Email}\" with hashed key \"{apiKey}\".");

            try
            {
                //Authorization with server

                HttpStatusCode code = await _httpClient.Login(_viewModel.Email, apiKey);

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
            catch (Exception e)
            {
                //Local mode authorization

                _viewModel.ErrorMessage = $"Have no connection to server";

                string localRepositoryKey = _encryptor.Encrypt(_viewModel.Password, _viewModel.Email, LOCAL_ITERATION_COUNT);

                bool isUserValid = _repository.AuthenticateLocally(localRepositoryKey, _viewModel.Email);
                if (isUserValid)
                {
                    _logger.Information($"User with E-mail {_viewModel.Email} is authenticated locally.");

                    bool? isLocalMode = _dialogService.ShowDialog("LocalModeDialog", null);
                    if (isLocalMode == true)
                    {
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

        private bool IsEmailValid(string emailaddress)
        {
            if (emailaddress == null)
            {
                return false;
            }

            try
            {
                new MailAddress(emailaddress);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
