using SnailPass_Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public LoginCommand(LoginViewModel viewModel, IUserRepository repository)
        {
            _viewModel = viewModel;
            _repository = repository;
        }

        public override bool CanExecute(object? parameter)
        {
            bool validData = false;
            Console.WriteLine("CanExe");

            if (IsEmailValid(_viewModel.Email) != false && _viewModel.Email.Length >= 5 &&
                _viewModel.Password != null && _viewModel.Password.Length >= 10)
            {
                validData = true;
            }

            return validData;
        }

        public override void Execute(object? parameter)
        {
            Console.WriteLine("Exe");
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential(_viewModel.Email, _viewModel.Password);
            var isValidUser = _repository.AuthenticateUser(credential);

            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(_viewModel.Username), null);
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
