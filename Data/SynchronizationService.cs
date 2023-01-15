using Microsoft.Extensions.Configuration;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Cryptography;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace SnailPass_Desktop.Data
{
    public class SynchronizationService : ISynchronizationService
    {
        private readonly int LOCAL_ITERATIONS_COUNT;

        private IRestClient _restClient;
        private IAccountRepository _accountRepository;
        private IUserRepository _userRepository;
        private IUserIdentityStore _identity;
        private IMasterPasswordEncryptor _encryptor;

        public SynchronizationService(IRestClient httpClient, IAccountRepository accountRepository, 
            IUserRepository userRepository, IUserIdentityStore identity, IMasterPasswordEncryptor encryptor)
        {
            LOCAL_ITERATIONS_COUNT = int.Parse(ConfigurationManager.AppSettings["local_hash_iterations"]);

            _restClient = httpClient;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _identity = identity;
            _encryptor = encryptor;
        }

        private async Task SynchronizeAccountsDataAsync(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            IEnumerable<AccountModel> accounts = await _restClient.GetAccounts();
            foreach (AccountModel account in accounts)
            {
                _accountRepository.AddOrReplace(account);
            }
        }

        private async Task SynchronizeUserDataAsync(string email)
         {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            UserModel user = await _restClient.GetUserAsync(email);
            user.Password = _encryptor.Encrypt(_identity.Master, email, LOCAL_ITERATIONS_COUNT);
            _userRepository.AddOrReplace(user);
        }

        public async Task SynchronizeAsync(string email)
        {
            Task user = SynchronizeUserDataAsync(email);
            Task accounts = SynchronizeAccountsDataAsync(email);
            await Task.WhenAll(user, accounts);
        }
    }
}
