using Serilog;
using Newtonsoft.Json;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Cryptography;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Commands
{
    internal class LoginCommand : CommandBase
    {
        private const int LOGIN_ITERATION_COUNT = 160000;

        LoginViewModel _viewModel;
        IUserIdentityStore _identity;
        IUserRepository _repository;
        IMasterPasswordEncryptor _encryptor;
        IRestClient _httpClient;
        ILogger _logger;

        public LoginCommand(LoginViewModel viewModel, IUserIdentityStore identityStore, IRestClient httpClient,
            IUserRepository repository, IMasterPasswordEncryptor encryptor, ILogger logger)
        {
            _viewModel = viewModel;
            _identity = identityStore;
            _httpClient = httpClient;
            _repository = repository;
            _encryptor = encryptor;
            _logger = logger;
        }

        public override bool CanExecute(object? parameter)
        {
            bool validData = false;

            if (IsEmailValid(_viewModel.Email) != false && _viewModel.Email.Length >= 5 &&
                _viewModel.Password != null && _viewModel.Password.Length >= 10)
            {
                validData = true;
            }

            return validData;
        }

        public override async void Execute(object? parameter)
        {
            _viewModel.ErrorMessage = null;
            string encryptedPassword = _encryptor.Encrypt(_viewModel.Password, _viewModel.Email, LOGIN_ITERATION_COUNT);

            HttpStatusCode code = await _httpClient.Login(_viewModel.Email, encryptedPassword);

            //Local mode? Dialog
            
            if (code == HttpStatusCode.OK)
            {

            }
            else
            {
                _viewModel.ErrorMessage = "Some error with code: " + code.ToString();
            }

            return;

            UserModel user = _httpClient.GetUser(_viewModel.Email);

            //if (user != null)
            //{
            //    _repository.Add(user);
            //}

            bool isValidUser = _repository.AuthenticateUser(encryptedPassword, _viewModel.Email);

            if (!string.IsNullOrEmpty(_httpClient.Token) && isValidUser == true)
            {
                _identity.Master = _viewModel.Password;
                _identity.CurrentUser = _repository.GetByEmail(_viewModel.Email);
                _viewModel.IsViewVisible = false;
            }
            else
            {
                _viewModel.ErrorMessage = "Invalid email or password";
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
