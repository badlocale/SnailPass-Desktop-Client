using SnailPass_Desktop.Model;
using System;
using System.Net.Mail;
using System.Net;
using Serilog;
using SnailPass_Desktop.Model.Interfaces;
using System.Configuration;
using SnailPass_Desktop.Model.Cryptography;
using System.Text;
using System.Security;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class RegistrationCommand : CommandBase
    {
        RegistrationViewModel _viewModel;
        IUserRestApi _userRestApi;
        IKeyGenerator _keyGenerator;
        ILogger _logger;

        public RegistrationCommand(RegistrationViewModel viewModel, IUserRestApi userRestApi,
            IKeyGenerator encryptor, ILogger logger)
        {
            _viewModel = viewModel;
            _userRestApi = userRestApi;
            _keyGenerator = encryptor;
            _logger = logger;
        }

        public override async void Execute(object? obj)
        {
            _viewModel.ErrorMessage = string.Empty;
            _viewModel.ID = Guid.NewGuid().ToString();

            UserModel user = new UserModel(_viewModel.ID, _viewModel.Email, _viewModel.Hint, null);

            string encryptedPassword = _keyGenerator.Encrypt(_viewModel.Password, _viewModel.Email, 
                CryptoConstants.NETWORK_ITERATIONS_COUNT);
            user.Password = encryptedPassword;

            _logger.Debug($"Execute regitration for E-mail: \"{user.Email}\"");

            HttpStatusCode? code = await _userRestApi.PostUserAsync(user);
            if (code == HttpStatusCode.Created)
            {
                //notification dialog todo
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
    }
}