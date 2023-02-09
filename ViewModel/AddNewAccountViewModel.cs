using Newtonsoft.Json.Linq;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.ViewModel.Stores;
using System;

namespace SnailPass_Desktop.ViewModel
{
    public class AddNewAccountViewModel : ErrorViewModel
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

        private IUserIdentityStore _identity;

        public string ServiceName
        {
            get { return _serviceName; }
            set 
            { 
                _serviceName = value;
                OnPropertyChanged();

                ClearErrors();
                if (string.IsNullOrWhiteSpace(_serviceName))
                {
                    AddError("Service name field cannot be empty.");
                }
                if (_serviceName.Length != _serviceName.Trim().Length)
                {
                    AddError("Service name have leading or trailing white-spaces.");
                }
            }
        }

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged();

                ClearErrors();
                if (string.IsNullOrWhiteSpace(_login))
                {
                    AddError("Login field cannot be empty.");
                }
                if (_login.Length != _login.Trim().Length)
                {
                    AddError("Login have leading or trailing white-spaces.");
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();

                ClearErrors();
                if (string.IsNullOrWhiteSpace(_password))
                {
                    AddError("Password field cannot be empty.");
                }
                if (_password.Length != _password.Trim().Length)
                {
                    AddError("Password have leading or trailing white-spaces.");
                }
            }
        }

        public AddNewAccountViewModel(IUserIdentityStore identity)
        {
            _identity = identity;

            _isFavorite = false;
            _isDeleted = false;
            _creationTime = DateTime.Now;
            _updateTime = DateTime.Now;
            _id = Guid.NewGuid().ToString();
            _userId = _identity.CurrentUser.ID;

            AddError("Please enter the service name", nameof(ServiceName));
            AddError("Please enter the login", nameof(Login));
            AddError("Please enter the password", nameof(Password));
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
