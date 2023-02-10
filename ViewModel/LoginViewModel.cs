using Serilog;
using Serilog.Core;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.Services;
using SnailPass_Desktop.ViewModel.Commands;
using SnailPass_Desktop.ViewModel.Stores;
using System.Security;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    public class LoginViewModel : ErrorViewModel
    {
        private string _email;
        private SecureString _password;

        private string _errorMessage;
        private bool _isViewVisible = true;

        private ILogger _logger;

        public ICommand LoginCommand { get; }
        public ICommand NavigateRegistrationCommand { get; }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value.Trim();
                OnPropertyChanged();
                Validate();
            }
        }

        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
                Validate();
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

        public LoginViewModel(INavigationStore navigationStore, IUserIdentityStore identityStore, 
            IUserRestApi userRestApi, IUserRepository repository, IKeyGenerator encryptor, 
            ILogger logger, IDialogService dialogService, ISynchronizationService synchronizationService, 
            IApplicationModeStore modeStore, IAuthenticationService authenticationService)
        {
            _logger = logger;

            LoginCommand = new LoginCommand(this, identityStore, authenticationService);
            NavigateRegistrationCommand = new NavigateCommand<RegistrationViewModel>(navigationStore, 
                () => new RegistrationViewModel(navigationStore, identityStore, userRestApi, repository, encryptor, logger, 
                dialogService, synchronizationService, modeStore, authenticationService), "Registration");

            //E-mail validation
            AddValidationRule(nameof(Email), "E-mail field cannot be empty.", () =>
            {
                return !string.IsNullOrEmpty(_email);
            });
            AddValidationRule(nameof(Email), "E-mail address cannot be longer than 255 symbols.", () =>
            {
                return _email?.Length < 256;
            });

            //Password validation
            AddValidationRule(nameof(Password), "Password field cannot be empty.", () =>
            {
                return !(_password?.Length == 0);
            });
            AddValidationRule(nameof(Password), "Password cannot be longer than 300 symbols.", () =>
            {
                return _password?.Length < 301;
            });

            Validate(null);
        }
    }
}
