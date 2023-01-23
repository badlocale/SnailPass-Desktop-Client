using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Configuration;
using System.Security;

namespace SnailPass_Desktop.ViewModel
{
    public class AddNewAccountViewModel : ViewModelBase
    {
        private string _errorMessage;

        private string _id;
        private string _serviceName;
        private string? _login;
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
            }
        }

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        public string Password
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

        public AddNewAccountViewModel(IUserIdentityStore identity)
        {
            _identity = identity;

            _isFavorite = false;
            _isDeleted = false;
            _creationTime = DateTime.Now;
            _updateTime = DateTime.Now;
            _id = Guid.NewGuid().ToString();
            _userId = _identity.CurrentUser.ID;
        }

        public AccountModel CreateModel()
        {
            AccountModel accountModel = new AccountModel();

            accountModel.ID = _id;
            accountModel.ServiceName = _serviceName;
            accountModel.Login = _login;
            accountModel.UserId = _userId;
            accountModel.IsFavorite = _isFavorite.ToString();
            accountModel.IsDeleted = _isDeleted.ToString();
            accountModel.CreationTime = _creationTime.ToString();
            accountModel.UpdateTime = _updateTime.ToString();
            accountModel.Password = _password;
            accountModel.Nonce = null;

            return accountModel;
        }
    }
}
