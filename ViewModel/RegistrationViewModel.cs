using Serilog;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Commands;
using SnailPass_Desktop.ViewModel.Services;
using SnailPass_Desktop.ViewModel.Stores;
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    public class RegistrationViewModel : ErrorViewModel
    {
        private string _id;
        private string _username;
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
                ValidateEmail();
            }
        }

        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
                ValidatePassword();
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
            IApplicationModeStore modeStore)
        {
            _logger = logger;

            RegistrationCommand = new RegistrationCommand(this, userRestApi, encryptor, logger);
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(navigationStore, 
                () => new LoginViewModel(navigationStore, identityStore, userRestApi, repository, encryptor, logger, 
                dialogService, synchronizationService, modeStore), "Logging");
        }

        public void ValidateEmail()
        {
            ClearErrors(nameof(Email));

            if (string.IsNullOrWhiteSpace(Email))
            {
                AddError("E-mail field cannot be empty.", nameof(Email));
                return;
            }

            if (!new EmailAddressAttribute().IsValid(_email))
            {
                AddError("E-mail address is not correct.", nameof(Email));
            }
        }

        public void ValidatePassword()
        {
            ClearErrors(nameof(Password));

            if (_password.Length == 0)
            {
                AddError("Password field cannot be empty.", nameof(Password));
                return;
            }

            int minPasswordLenght = 10;
            if (_password.Length < minPasswordLenght)
            {
                AddError($"Password cannot be less than {minPasswordLenght} symbols", nameof(Password));
            }
        }
    }
}
