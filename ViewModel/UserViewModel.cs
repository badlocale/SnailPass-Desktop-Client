using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnailPass_Desctop.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        private string _username;
        private string _email;
        private SecureString _password;
        private string _hint;
        private string _errorMessage;
        private bool _isViewVisible = true;

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

        public string Hint
        {
            get { return _hint; }
            set
            {
                _hint = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand RegistrationCommand { get; }

        public UserViewModel()
        {
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RegistrationCommand = new ViewModelCommand(ExecuteRegistrationCommand, CanExecuteRegistrationCommand);
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData = false;
            if (IsEmailValid(Email) != false && Email.Length >= 5 && Password != null && Password.Length >= 10)
                validData = true;
            return validData;
        }

        private void ExecuteLoginCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanExecuteRegistrationCommand(object obj)
        {
            bool isValidData = false;
            if (IsEmailValid(Email) != false && Password != null)
                isValidData = true;
            return isValidData;
        }

        private void ExecuteRegistrationCommand(object obj)
        {
            throw new NotImplementedException();
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
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
