using SnailPass_Desktop.Model;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel
{
    public class EditAccountViewModel : ViewModelBase
    {
        private string? _serviceName;
        private string? _login;
        private string? _password;

        private string _errorMessage = string.Empty;

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

        public AccountModel CreateModel()
        {
            AccountModel accountModel = new();

            accountModel.ServiceName = _serviceName;
            accountModel.Login = _login;
            accountModel.Password = _password;

            return accountModel;
        }
    }
}
