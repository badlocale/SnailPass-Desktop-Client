using SnailPass_Desktop.Model;
using SnailPass_Desktop.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    internal class RegistrationViewModel : ViewModelBase
    {
        private string _id = Guid.NewGuid().ToString();
        private string _username = "iZelton";
        private string _email = "shade.of.apple@gmail.com";
        private string _hint = "PUDGE";
        private string _nonce = "3234";

        private SecureString _password;

        private string _errorMessage;
        private bool _isViewVisible = true;

        private IUserRepository _userRepository;

        public ICommand RegistrationCommand { get; }

        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string Hint
        {
            get { return _hint; }
            set
            {
                _hint = value;
                OnPropertyChanged();
            }
        }

        public string Nonce
        {
            get { return _nonce; }
            set
            {
                _nonce = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged();
            }
        }

        public RegistrationViewModel()
        {
            _userRepository = new UserRepository();
            RegistrationCommand = new ViewModelCommand(ExecuteRegistrationCommand, CanExecuteRegistrationCommand);
        }

        private bool CanExecuteRegistrationCommand(object obj)
        {
            bool isValidData = false;
            if (IsEmailValid(Email) != false && Email.Length >= 5 && Password != null && Password.Length >= 10)
            {
                isValidData = true;
            }
            return isValidData;
        }

        private void ExecuteRegistrationCommand(object obj)
        {
            //TODO make reg with server
            UserModel newUser = _userRepository.GetByEmail(Email);

            if (newUser != null)
            {
                ErrorMessage = "user with such email already exists";
                return;
            }

            //TODO encrypt pass
            UserModel user = new UserModel(ID, Username, Email, Hint, Nonce);

            _userRepository.Add(user);
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
