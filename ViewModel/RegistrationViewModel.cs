using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnailPass_Desctop.ViewModel
{
    internal class RegistrationViewModel : ViewModelBase
    {
        private string _username;
        private string _email;
        private SecureString _password;
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

        public ICommand RegistrationCommand { get; }

        public RegistrationViewModel()
        {
            RegistrationCommand = new ViewModelCommand(ExecuteRegistrationCommand, CanExecuteRegistrationCommand);
        }

        private bool CanExecuteRegistrationCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private void ExecuteRegistrationCommand(object obj)
        {

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
