using Serilog;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.Services;
using SnailPass_Desktop.ViewModel.Commands;
using SnailPass_Desktop.ViewModel.Stores;
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    public class RegistrationViewModel : ErrorViewModel
    {
        private string _id;
        private string _email;
        private string _hint;

        private SecureString _password;

        private string _errorMessage;
        private bool _isViewVisible = true;

        private ILogger _logger;

        public ICommand RegistrationCommand { get; }
        public ICommand NavigateLoginCommand { get; }

        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }

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

        public string Hint
        {
            get { return _hint; }
            set
            {
                _hint = value;
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
                _isViewVisible = value;
                OnPropertyChanged();
            }
        }

        public RegistrationViewModel(INavigationStore navigationStore, IUserIdentityStore identityStore, 
            IUserRestApi userRestApi, IUserRepository repository, IKeyGenerator encryptor, 
            ILogger logger, IDialogService dialogService, ISynchronizationService synchronizationService, 
            IApplicationModeStore modeStore, IAuthenticationService authenticationService)
        {
            _logger = logger;

            RegistrationCommand = new RegistrationCommand(this, userRestApi, encryptor, logger, authenticationService);
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(navigationStore, 
                () => new LoginViewModel(navigationStore, identityStore, userRestApi, repository, encryptor, logger, 
                dialogService, synchronizationService, modeStore, authenticationService), "Logging");

            //E-mail validation
            AddValidationRule(nameof(Email), "E-mail field cannot be empty.", () =>
            {
                return !string.IsNullOrEmpty(_email);
            });
            AddValidationRule(nameof(Email), "E-mail have leading or trailing white-spaces.", () =>
            {
                return _email?.Length == _email?.Trim().Length;
            });
            AddValidationRule(nameof(Email), "Entered text is not e-mail.", () =>
            {
                return new EmailAddressAttribute().IsValid(_email);
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
            AddValidationRule(nameof(Password), "Password cannot be less than 10 symbols.", () =>
            {
                return _password?.Length > 10;
            });
            AddValidationRule(nameof(Password), "Password cannot be longer than 300 symbols.", () =>
            {
                return _password?.Length < 301;
            });

            Validate(null);
        }
    }
}
