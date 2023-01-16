using Serilog;
using Serilog.Core;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.Repositories;
using SnailPass_Desktop.ViewModel.Commands;
using SnailPass_Desktop.ViewModel.Services;
using SnailPass_Desktop.ViewModel.Stores;
using System.Security;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _id;
        private string _email;
        private string? _hint;

        private SecureString _password;

        private string _errorMessage;
        private bool _isViewVisible = true;

        private ILogger _logger;

        public ICommand LoginCommand { get; }
        public ICommand NavigateRegistrationCommand { get; }

        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
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
            IRestClient httpClient, IUserRepository repository, IMasterPasswordEncryptor encryptor, 
            ILogger logger, IDialogService dialogService, ISynchronizationService synchronizationService, 
            IApplicationModeStore modeStore)
        {
            _logger = logger;

            LoginCommand = new LoginCommand(this, identityStore, httpClient, repository, encryptor, logger, dialogService, 
                synchronizationService, modeStore);
            NavigateRegistrationCommand = new NavigateCommand<RegistrationViewModel>(navigationStore, 
                () => new RegistrationViewModel(navigationStore, identityStore, httpClient, repository, encryptor, logger, 
                dialogService, synchronizationService, modeStore), "Registration");
        }
    }
}
