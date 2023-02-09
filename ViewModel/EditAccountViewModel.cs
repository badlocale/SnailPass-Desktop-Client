using SnailPass_Desktop.Model;

namespace SnailPass_Desktop.ViewModel
{
    public class EditAccountViewModel : ErrorViewModel
    {
        private string? _serviceName;
        private string? _login;
        private string? _password;

        public string ServiceName
        {
            get { return _serviceName; }
            set
            {
                _serviceName = value;
                OnPropertyChanged();

                ClearErrors();
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
                if (_password.Length != _password.Trim().Length)
                {
                    AddError("Password have leading or trailing white-spaces.");
                }
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
