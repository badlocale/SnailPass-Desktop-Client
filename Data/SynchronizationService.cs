using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Cryptography;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace SnailPass_Desktop.Data
{
    public class SynchronizationService : ISynchronizationService
    {
        private IRestClient _restClient;
        private IAccountRepository _accountRepository;
        private IUserRepository _userRepository;
        private IUserIdentityStore _identity;
        private IMasterPasswordEncryptor _encryptor;
        private IApplicationModeStore _modeStore;
        private ILogger _logger;

        public SynchronizationService(IRestClient httpClient, IAccountRepository accountRepository, 
            IUserRepository userRepository, IUserIdentityStore identity, IMasterPasswordEncryptor encryptor,
            IApplicationModeStore modeStore, ILogger logger)
        {
            _restClient = httpClient;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _identity = identity;
            _encryptor = encryptor;
            _modeStore = modeStore;
            _logger = logger;
        }

        private async Task SynchronizeAccountsDataAsync(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            IEnumerable<AccountModel> accounts;
            (_, accounts) = await _restClient.GetAccountsAsync();

            if (accounts != null)
            {
                foreach (AccountModel account in accounts)
                {
                    _accountRepository.AddOrReplace(account);
                }
            }
        }

        private async Task SynchronizeUserDataAsync(string email)
         {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            UserModel user;
            (_, user) = await _restClient.GetUserAsync(email);
            user.Password = _encryptor.Encrypt(_identity.Master, email, CryptoConstants.LOCAL_ITERATIONS_COUNT);
            _userRepository.AddOrReplace(user);
        }

        public async Task SynchronizeAsync(string email)
        {
            if (!_modeStore.IsLocalMode)
            {
                Task user = SynchronizeUserDataAsync(email);
                Task accounts = SynchronizeAccountsDataAsync(email);
                await Task.WhenAll(user, accounts).ConfigureAwait(false);
                _logger.Information("Data has been loaded from server.");
            }
            else
            {
                _logger.Information("Can`t synchronize, client in local mode.");
            }
        }
    }
}
