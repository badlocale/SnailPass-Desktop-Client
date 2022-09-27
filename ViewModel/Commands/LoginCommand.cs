using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Cryptography;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Commands
{
    internal class LoginCommand : CommandBase
    {
        LoginViewModel _viewModel;
        UserIdentityStore _identityStore;
        IUserRepository _repository;
        IMasterPasswordEncryptor _encryptor;

        public LoginCommand(LoginViewModel viewModel, UserIdentityStore identityStore, 
            IUserRepository repository, IMasterPasswordEncryptor encryptor)
        {
            _viewModel = viewModel;
            _identityStore = identityStore;
            _repository = repository;
            _encryptor = encryptor;
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

        public override void Execute(object? parameter)
        {
            string encryptedPassword = _encryptor.Encrypt(_viewModel.Password, _viewModel.Email, 200000);
            
            bool isValidUser = _repository.AuthenticateUser(encryptedPassword, _viewModel.Email);

            if (isValidUser)
            {
                _identityStore.Master = _viewModel.Password;
                _identityStore.CurrentUser = _repository.GetByEmail(_viewModel.Email);
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
