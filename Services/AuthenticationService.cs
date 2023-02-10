using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Cryptography;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IKeyGenerator _keyGenerator;
        private ILogger _logger;
        private IUserRestApi _userRestApi;
        private IUserIdentityStore _identity;
        private ISynchronizationService _synchronizationService;
        private IUserRepository _userRepository;
        private IDialogService _dialogService;
        private IApplicationModeStore _applicationModeStore;

        public AuthenticationService(IKeyGenerator keyGenerator, ILogger logger, IUserRestApi userRestApi,
            IUserIdentityStore identity, ISynchronizationService synchronizationService, IUserRepository userRepository,
            IDialogService dialogService, IApplicationModeStore applicationModeStore)
        {
            _keyGenerator = keyGenerator;
            _logger = logger;
            _userRestApi = userRestApi;
            _identity = identity;
            _synchronizationService = synchronizationService;
            _userRepository = userRepository;
            _dialogService = dialogService;
            _applicationModeStore = applicationModeStore;
        }

        public async Task<RegistrationResult> Register(string email, SecureString password, string hint)
        {
            string? errorMessage;
            bool isSuccess;

            string id = Guid.NewGuid().ToString();
            string encryptedPassword = _keyGenerator.Encrypt(password, email,
                CryptoConstants.NETWORK_ITERATIONS_COUNT);
            UserModel user = new UserModel(id, email, hint, encryptedPassword);

            _logger.Debug($"Execute regitration for E-mail: \"{user.Email}\"");

            HttpStatusCode? code = await _userRestApi.PostUserAsync(user);
            errorMessage = null;
            isSuccess = false;
            if (code == HttpStatusCode.Created)
            {
                isSuccess = true;
                _logger.Information($"Successful registration for: {user.Email}.");
            }
            else if (code == HttpStatusCode.Conflict)
            {
                errorMessage = "User with such E-mail is already exists";
                _logger.Information($"Such E-mail already registered. Http code: {code}.");
            }
            else if (code == HttpStatusCode.BadRequest)
            {
                errorMessage = "This E-mail is not valid for registration";
                _logger.Information($"Non-valid data for registraion. Http code: {code}.");
            }
            else if (code != null)
            {
                errorMessage = $"Some error with code \"{code}\"";
                _logger.Error($"Unexpected http code. Http code: {code}.");
            }
            else
            {
                errorMessage = $"Some trouble with server connection.";
                _logger.Error($"Null http code. Server not avalible.");
            }

            return new RegistrationResult(isSuccess, errorMessage);
        }

        public async Task<LoggingResult> Login(string email, SecureString password)
        {
            _logger.Information($"Execute logging for E-mail: \"{email}\".");

            LoggingResult viaNetworkResult = await LoginViaNetwork(email, password);
            if (viaNetworkResult.IsSuccess)
            {
                return viaNetworkResult;
            }

            LoggingResult localResult = LoginLocally(email, password);
            if (localResult.IsSuccess)
            {
                return localResult;
            }
            else
            {
                return viaNetworkResult;
            }
        }

        private async Task<LoggingResult> LoginViaNetwork(string email, SecureString password)
        {
            string? errorMessage = null;
            bool isSuccess = false;
            bool isLocally = false;
            UserModel? user = null;

            string apiKey = _keyGenerator.Encrypt(password, email,
                CryptoConstants.NETWORK_ITERATIONS_COUNT);

            HttpStatusCode? code = await _userRestApi.LoginAsync(email, apiKey);
            if (code == HttpStatusCode.OK)
            {
                await _synchronizationService.SynchronizeAsync(email);
                user = _userRepository.GetByEmail(email);

                if (user == null)
                {
                    errorMessage = "Can't find such user in local storage";
                    _logger.Error($"User [{email}] not in local storage after synchronization.");
                }
                else
                {
                    isSuccess = true;
                    _logger.Error($"Successful logging for: {email}.");
                }
            }
            else if (code == HttpStatusCode.Unauthorized)
            {
                errorMessage = $"Not correct E-mail or password";
                _logger.Error($"Not corrent E-mail or password. Http code: {code}.");
            }
            else if (code != null)
            {
                errorMessage = $"Some error with code \"{code}\"";
                _logger.Error($"Unexpected http code. Http code: {code}.");
            }
            else
            {
                errorMessage = $"Some trouble with server connection.";
                _logger.Error($"Null http code. Server not avalible.");
            }

            return new LoggingResult(isSuccess, isLocally, errorMessage, user);
        }

        private LoggingResult LoginLocally(string email, SecureString password)
        {
            bool isSuccess = false;
            UserModel? user = null;

            string localRepositoryKey = _keyGenerator.Encrypt(password, email,
                CryptoConstants.LOCAL_ITERATIONS_COUNT);

            _logger.Information($"Try to local authentication.");

            bool isUserValid = _userRepository.AuthenticateLocally(localRepositoryKey, email);
            if (isUserValid)
            {
                _logger.Information($"User with E-mail [{email}] is authenticated locally.");

                bool? isLocalMode = _dialogService.ShowDialog("LocalModeDialog");
                if (isLocalMode == true)
                {
                    _applicationModeStore.IsLocalMode = true;
                    user = _userRepository.GetByEmail(email);

                    if (user == null)
                    {
                        _logger.Information($"Can't get user data in local storage.");
                    }
                    else
                    {
                        isSuccess = true;
                    }
                }
            }
            else
            {
                _logger.Information($"User with E-mail [{email}] is not authenticated locally.");
            }

            return new LoggingResult(isSuccess, true, null, user);
        }
    }

    public class RegistrationResult
    {
        public RegistrationResult(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class LoggingResult
    {
        public LoggingResult(bool isSuccess, bool isLocally, string? errorMessage,
            UserModel? user)
        {
            IsSuccess = isSuccess;
            IsLocally = isLocally;
            ErrorMessage = errorMessage;
            User = user;
        }

        public bool IsSuccess { get; set; }
        public bool IsLocally { get; set; }
        public string? ErrorMessage { get; set; }
        public UserModel? User { get; set; }
    }
}