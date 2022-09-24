using SnailPass_Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Commands
{
    internal class RegistrationCommand : CommandBase
    {
        RegistrationViewModel _viewModel;
        IUserRepository _repository;

        public RegistrationCommand(RegistrationViewModel viewModel, IUserRepository repository)
        {
            _viewModel = viewModel;
            _repository = repository;
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

        public override void Execute(object? obj)
        {
            //TODO make reg with server
            UserModel user = _repository.GetByEmail(_viewModel.Email);

            if (user != null)
            {
                _viewModel.ErrorMessage = "user with such email already exists";
                return;
            }

            //TODO encrypt pass
            user = new UserModel(_viewModel.ID, _viewModel.Username, _viewModel.Email, _viewModel.Hint, _viewModel.Nonce);

            _repository.Add(user);
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