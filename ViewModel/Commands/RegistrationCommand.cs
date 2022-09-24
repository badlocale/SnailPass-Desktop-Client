using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Cryptography;
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
        IMasterPasswordEncryptor _encryptor;

        public RegistrationCommand(RegistrationViewModel viewModel, IUserRepository repository, IMasterPasswordEncryptor encryptor)
        {
            _viewModel = viewModel;
            _repository = repository;
            _encryptor = encryptor;
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
            _viewModel.ErrorMessage = null;
            //TODO make reg with server

            if (_repository.IsEmailExist(_viewModel.Email) == true)
            {
                _viewModel.ErrorMessage = "User with such email already exists";
                return;
            }

            if (_repository.IsUsernameExist(_viewModel.Username) == true)
            {
                _viewModel.ErrorMessage = "Such username is already in use";
                return;
            }

            _viewModel.ID = Guid.NewGuid().ToString();

            string encryptedPassword = _encryptor.Encrypt(_viewModel.Password, _viewModel.Email, 200000);

            UserModel user = new UserModel(_viewModel.ID, _viewModel.Username, _viewModel.Email, _viewModel.Hint, _viewModel.Nonce, encryptedPassword);

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