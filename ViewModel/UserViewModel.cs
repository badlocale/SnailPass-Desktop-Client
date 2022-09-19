using SnailPass_Desctop.Model;
using SnailPass_Desctop.Repositories;
using System;
using System.Net.Mail;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Windows.Input;

namespace SnailPass_Desctop.ViewModel
{
    public class UserViewModel : ViewModelBase
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

        public ICommand LoginCommand { get; }
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

        public UserViewModel()
        {
            _userRepository = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RegistrationCommand = new ViewModelCommand(ExecuteRegistrationCommand, CanExecuteRegistrationCommand);
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData = false;
            if (IsEmailValid(Email) != false && Email.Length >= 5 && Password != null && Password.Length >= 10)
            {
                validData = true;
            }
                
            return validData;
        }

        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = _userRepository.AuthenticateUser(new System.Net.NetworkCredential(Username, Password));
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "Invalid email or password";
            }
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
