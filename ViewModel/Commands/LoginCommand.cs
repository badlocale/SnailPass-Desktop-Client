using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Cryptography;
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
        IUserRepository _repository;
        IMasterPasswordEncryptor _encryptor;

        public LoginCommand(LoginViewModel viewModel, IUserRepository repository, IMasterPasswordEncryptor encryptor)
        {
            _viewModel = viewModel;
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
                //Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(_viewModel.Username), null);
                _viewModel.IsViewVisible = false; //TODO new window navigation
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
