using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Cryptography;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Services
{
    public class SynchronizationService : ISynchronizationService
    {
        private IUserRestApi _userRestApi;
        private IAccountRestApi _accountRestApi;
        private ICustomFieldRestApi _customFieldRestApi;
        private ICustomFieldRepository _customFieldRepository;
        private IUserRepository _userRepository;
        private IAccountRepository _accountRepository;
        private IUserIdentityStore _identity;
        private IKeyGenerator _keyGenerator;
        private IApplicationModeStore _modeStore;
        private ILogger _logger;

        public SynchronizationService(IUserRestApi userRestApi, IAccountRestApi accountRestApi,
            ICustomFieldRestApi customFieldRestApi, IAccountRepository accountRepository,
            ICustomFieldRepository customFieldRepository, IUserRepository userRepository,
            IUserIdentityStore identity, IKeyGenerator encryptor,
            IApplicationModeStore modeStore, ILogger logger)
        {
            _userRestApi = userRestApi;
            _accountRestApi = accountRestApi;
            _customFieldRestApi = customFieldRestApi;
            _customFieldRepository = customFieldRepository;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _identity = identity;
            _keyGenerator = encryptor;
            _modeStore = modeStore;
            _logger = logger;
        }

        private async Task SynchronizeFieldsDataAsync(string accountID)
        {
            if (accountID == null)
            {
                throw new ArgumentNullException(nameof(accountID));
            }

            IEnumerable<EncryptableFieldModel?> fields;
            (_, fields) = await _customFieldRestApi.GetCustomFieldsAsync(accountID);

            if (fields != null)
            {
                _accountRepository.DeleteAllByAccountID(accountID);

                foreach (EncryptableFieldModel account in fields)
                {
                    _customFieldRepository.AddOrReplace(account);
                }
            }
        }

        private async Task SynchronizeAccountsDataAsync(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            IEnumerable<AccountModel> accounts;
            (_, accounts) = await _accountRestApi.GetAccountsAsync();

            if (accounts != null)
            {
                _accountRepository.DeleteAllByAccountID(email);

                foreach (AccountModel account in accounts)
                {
                    _accountRepository.AddOrReplace(account);
                    await SynchronizeFieldsDataAsync(account.ID);
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
            (_, user) = await _userRestApi.GetUserAsync(email);
            user.Password = _keyGenerator.Encrypt(_identity.Master, email, 
                CryptoConstants.LOCAL_ITERATIONS_COUNT);
            _userRepository.AddOrReplace(user);
        }

        public async Task SynchronizeAsync(string email)
        {
            if (!_modeStore.IsLocalMode)
            {
                _logger.Information("Synchronization started.");

                Task user = SynchronizeUserDataAsync(email);
                Task accounts = SynchronizeAccountsDataAsync(email);
                Task aggregateTask = Task.WhenAll(user, accounts);
                await aggregateTask.ConfigureAwait(false);
                aggregateTask.Wait();

                _logger.Information("Data has been loaded from server.");
            }
            else
            {
                _logger.Information("Can`t synchronize, client in local mode.");
            }
        }
    }
}