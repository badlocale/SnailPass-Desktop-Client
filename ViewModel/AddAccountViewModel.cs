using Autofac.Core;
using Newtonsoft.Json.Linq;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Commands;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Windows.Input;
using System.Xml.Linq;

namespace SnailPass_Desktop.ViewModel
{
    public class AddAccountViewModel : ErrorViewModel
    {
        private string _id;
        private string _serviceName;
        private string _login;
        private bool _isFavorite;
        private bool _isDeleted;
        private DateTime _creationTime;
        private DateTime _updateTime;
        private string _userId;
        private string _password;

        private int _lenght;
        private bool _isLowercase;
        private bool _isUppercase;
        private bool _isDigits;
        private bool _isSpecials;

        public string ServiceName
        {
            get { return _serviceName; }
            set 
            { 
                _serviceName = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public int Lenght
        {
            get { return _lenght; }
            set
            {
                _lenght = (int)Math.Round((double)value, 0);
                OnPropertyChanged();
            }
        }

        public bool IsLowercase
        {
            get { return _isLowercase; }
            set
            {
                _isLowercase = value;
                OnPropertyChanged();
            }
        }

        public bool IsUppercase
        {
            get { return _isUppercase; }
            set
            {
                _isUppercase = value;
                OnPropertyChanged();
            }
        }

        public bool IsDigits
        {
            get { return _isDigits; }
            set
            {
                _isDigits = value;
                OnPropertyChanged();
            }
        }

        public bool IsSpecials
        {
            get { return _isSpecials; }
            set
            {
                _isSpecials = value;
                OnPropertyChanged();
            }
        }

        public ICommand GeneratePasswordCommand { get; set; }

        public AddAccountViewModel(IUserIdentityStore identity, IPasswordGenerator passwordGenerator)
        {
            int defaultPassLength = 6;
            Lenght = defaultPassLength;

            _isFavorite = false;
            _isDeleted = false;
            _creationTime = DateTime.Now;
            _updateTime = DateTime.Now;
            _id = Guid.NewGuid().ToString();
            _userId = identity.CurrentUser.ID;

            GeneratePasswordCommand = new GeneratePasswordCommand(this, passwordGenerator);

            //Service name validation
            AddValidationRule(nameof(ServiceName), "Service name field cannot be empty.", () =>
            {
                return !string.IsNullOrEmpty(_serviceName);
            });
            AddValidationRule(nameof(ServiceName), "Service name have leading or trailing white-spaces.", () =>
            {
                return _serviceName?.Length == _serviceName?.Trim().Length;
            });
            AddValidationRule(nameof(ServiceName), "Service name cannot be longer than 300 symbols.", () =>
            {
                return _serviceName?.Length < 301;
            });

            //Login validation
            AddValidationRule(nameof(Login), "Login field cannot be empty.", () =>
            {
                return !string.IsNullOrEmpty(_login);
            });
            AddValidationRule(nameof(Login), "Login have leading or trailing white-spaces.", () =>
            {
                return _login?.Length == _login?.Trim().Length;
            });
            AddValidationRule(nameof(Login), "Login cannot be longer than 300 symbols.", () =>
            {
                return _login?.Length < 301;
            });

            //Password validation
            AddValidationRule(nameof(Password), "Password field cannot be empty.", () =>
            {
                return !string.IsNullOrEmpty(_password);
            });
            AddValidationRule(nameof(Password), "Password have leading or trailing white-spaces.", () =>
            {
                return _password?.Length == _password?.Trim().Length;
            });
            AddValidationRule(nameof(Password), "Password cannot be longer than 300 symbols.", () =>
            {
                return _password?.Length < 301;
            });

            Validate(null);
        }

        public AccountModel CreateModel()
        {
            AccountModel accountModel = new AccountModel();

            accountModel.ID = _id;
            accountModel.ServiceName = _serviceName;
            accountModel.Login = _login;
            accountModel.UserId = _userId;
            accountModel.IsFavorite = _isFavorite;
            accountModel.IsDeleted = _isDeleted;
            accountModel.CreationTime = _creationTime.ToString();
            accountModel.UpdateTime = _updateTime.ToString();
            accountModel.Password = _password;

            return accountModel;
        }
    }
}
