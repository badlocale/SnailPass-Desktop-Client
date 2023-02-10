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

        public EditAccountViewModel()
        {
            //Service name validation
            AddValidationRule(nameof(ServiceName), "Service name have leading or trailing white-spaces.", () =>
            {
                if (_serviceName == null)
                    return true;
                return _serviceName?.Length == _serviceName?.Trim().Length; ;
            });
            AddValidationRule(nameof(ServiceName), "Service name cannot be longer than 300 symbols.", () =>
            {
                if (_serviceName == null)
                    return true;
                return _serviceName?.Length < 301;
            });

            //Login validation
            AddValidationRule(nameof(Login), "Login have leading or trailing white-spaces.", () =>
            {
                if (_login == null)
                    return true;
                return _login?.Length == _login?.Trim().Length;
            });
            AddValidationRule(nameof(Login), "Login cannot be longer than 300 symbols.", () =>
            {
                if (_login == null)
                    return true;
                return _login?.Length < 301;
            });

            //Password validation
            AddValidationRule(nameof(Password), "Password have leading or trailing white-spaces.", () =>
            {
                if (_password == null)
                    return true;
                return _password?.Length == _password?.Trim().Length; ;
            });
            AddValidationRule(nameof(Password), "Password cannot be longer than 300 symbols.", () =>
            {
                if (_password == null)
                    return true;
                return _password?.Length < 301;
            });

            Validate(null);
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
