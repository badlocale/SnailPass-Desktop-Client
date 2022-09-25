using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Cryptography;
using SnailPass_Desktop.Repositories;
using SnailPass_Desktop.ViewModel.Commands;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _id;
        private string _username;
        private string _email;
        private string? _hint;
        private string? _nonce;

        private SecureString _password;

        private string _errorMessage;
        private bool _isViewVisible = true;

        private IUserRepository _repository;
        private IMasterPasswordEncryptor _encryptor;

        public ICommand LoginCommand { get; }
        public ICommand NavigationRegistrationCommand { get; }

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
                bool oldValue = _isViewVisible;
                if (oldValue != value)
                {
                    _isViewVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public LoginViewModel(NavigationStore navigationStore)
        {
            _repository = new UserRepository();
            _encryptor = new Pbkdf2Encryptor();
            LoginCommand = new LoginCommand(this, _repository, _encryptor);
            NavigationRegistrationCommand = new NavigationRegistrationCommand(navigationStore);
        }
    }
}
