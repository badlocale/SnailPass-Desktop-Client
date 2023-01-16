using SnailPass_Desktop.Model;
using System;
using System.Net.Mail;
using System.Net;
using Serilog;
using SnailPass_Desktop.Model.Interfaces;
using System.Configuration;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class RegistrationCommand : CommandBase
    {
        private readonly int NETWORK_ITERATION_COUNT;

        RegistrationViewModel _viewModel;
        IRestClient _httpClient;
        IMasterPasswordEncryptor _encryptor;
        ILogger _logger;

        public RegistrationCommand(RegistrationViewModel viewModel, IRestClient httpClient, 
            IMasterPasswordEncryptor encryptor, ILogger logger)
        {
            NETWORK_ITERATION_COUNT = int.Parse(ConfigurationManager.AppSettings["network_hash_iterations"]);

            _viewModel = viewModel;
            _httpClient = httpClient;
            _encryptor = encryptor;
            _logger = logger;
        }

        public override bool CanExecute(object? obj)
        {
            bool isValidData = false;
            if (IsEmailValid(_viewModel.Email) != false && _viewModel.Email.Length >= 5 && 
                _viewModel.Password != null && _viewModel.Password.Length >= 10)
            {
                isValidData = true;
            }
            return isValidData;
        }

        public override async void Execute(object? obj)
        {
            _viewModel.ErrorMessage = null;
            _viewModel.ID = Guid.NewGuid().ToString();

            string encryptedPassword = _encryptor.Encrypt(_viewModel.Password, _viewModel.Email, NETWORK_ITERATION_COUNT);
            UserModel user = new UserModel(_viewModel.ID, _viewModel.Email, _viewModel.Hint, encryptedPassword);

            _logger.Debug($"Execute regitration for E-mail: \"{user.Email}\"");

            HttpStatusCode code = await _httpClient.PostUserAsync(user);
            if (code == HttpStatusCode.Created)
            {
                //notification dialog
                _logger.Information($"Successful registration for: {user.Email}.");
            }
            else if (code == HttpStatusCode.Conflict)
            {
                _viewModel.ErrorMessage = "User with such E-mail is already exists";
                _logger.Information($"Such E-mail already registered: {user.Email}.");
            }
            else if (code == HttpStatusCode.BadRequest)
            {
                _viewModel.ErrorMessage = "This E-mail is not valid for registration";
                _logger.Information($"Non-valid E-mail: {user.Email}.");
            }
            else
            {
                _viewModel.ErrorMessage = $"Some error with code \"{code}\"";
                _logger.Error($"Some error. Registration request. Http code: {code}.");
            }
        }

        public bool IsEmailValid(string emailaddress)
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