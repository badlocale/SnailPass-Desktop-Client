using Serilog;
using SnailPass.Model;
using SnailPass.Model.Cryptography;
using SnailPass.Model.Interfaces;
using SnailPass.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnailPass.Services
{
    public class SynchronizationService : ISynchronizationService
    {
        private IUserRestApi _userRestApi;
        private IAccountRestApi _accountRestApi;
        private ICustomFieldRestApi _customFieldRestApi;
        private INoteRestApi _noteRestApi;
        private ICustomFieldRepository _customFieldRepository;
        private IUserRepository _userRepository;
        private IAccountRepository _accountRepository;
        private INoteRepository _noteRepository;
        private IUserIdentityStore _identity;
        private IKeyGenerator _keyGenerator;
        private ILogger _logger;

        public SynchronizationService(IUserRestApi userRestApi, IAccountRestApi accountRestApi, 
            INoteRestApi noteRestApi, ICustomFieldRestApi customFieldRestApi, IAccountRepository accountRepository, 
            INoteRepository noteRepository, ICustomFieldRepository customFieldRepository, IUserRepository userRepository,
            IUserIdentityStore identity, IKeyGenerator encryptor, ILogger logger)
        {
            _userRestApi = userRestApi;
            _accountRestApi = accountRestApi;
            _customFieldRestApi = customFieldRestApi;
            _noteRestApi = noteRestApi;
            _customFieldRepository = customFieldRepository;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _noteRepository = noteRepository;
            _identity = identity;
            _keyGenerator = encryptor;
            _logger = logger;
        }

        private async Task SynchronizeFieldsDataAsync(string accountID)
        {
            try
            {
                if (accountID == null)
                {
                    throw new ArgumentNullException(nameof(accountID));
                }

                IEnumerable<EncryptableFieldModel>? fields;
                (_, fields) = await _customFieldRestApi.GetCustomFieldsAsync(accountID);

                if (fields != null)
                {
                    _customFieldRepository.RepaceAll(fields, accountID);
                }
                else
                {
                    throw new NullReferenceException("Null fields list returned from API.");
                }
            }
            catch
            {
                throw;
            }
        }

        private async Task SynchronizeAccountsDataAsync(string email)
        {
            try
            {
                if (email == null)
                {
                    throw new ArgumentNullException(nameof(email));
                }

                IEnumerable<AccountModel>? accounts;
                (_, accounts) = await _accountRestApi.GetAccountsAsync();

                if (accounts != null)
                {
                    _logger.Debug($"Sync serivce: {accounts.Count()} accounts loaded from server.");

                    _accountRepository.RepaceAll(accounts, email);

                    List<Task> tasks = new List<Task>();
                    foreach (AccountModel account in accounts)
                    {
                        tasks.Add(SynchronizeFieldsDataAsync(account.ID));
                    }
                    await Task.WhenAll(tasks).ConfigureAwait(false);
                }
                else
                {
                    throw new NullReferenceException("Null accounts list returned from API.");
                }
            }
            catch
            {
                throw;
            }
        }

        private async Task SynchronizeUserDataAsync(string email)
        {
            try
            {
                if (email == null)
                {
                    throw new ArgumentNullException(nameof(email));
                }

                UserModel? user;
                (_, user) = await _userRestApi.GetUserAsync(email);

                if (user != null)
                {
                    user.Password = _keyGenerator.Encrypt(_identity.Master, email, 
                        CryptoConstants.LOCAL_ITERATIONS_COUNT);

                    _userRepository.AddOrReplace(user);
                }
                else
                {
                    throw new NullReferenceException("Null user returned from API.");
                }
            }
            catch
            {
                throw;
            }
        }

        private async Task SynchronizeNotesDataAsync(string email)
        {
            try
            {
                if (email == null)
                {
                    throw new ArgumentNullException(nameof(email));
                }

                IEnumerable<NoteModel>? notes;
                (_, notes) = await _noteRestApi.GetNotesAsync();

                if (notes != null)
                {
                    _logger.Debug($"Sync serivce: {notes.Count()} notes loaded from server.");

                    _noteRepository.ReplaceAll(notes, email);
                }
                else
                {
                    throw new NullReferenceException("Null notes list returned from API.");
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SynchronizeAsync(string email)
        {
            try
            {
                bool isSuccess = false;

                _logger.Information("Synchronization started.");

                Task userTask = SynchronizeUserDataAsync(email);
                Task accountTask = SynchronizeAccountsDataAsync(email);
                Task noteTask = SynchronizeNotesDataAsync(email);
                await userTask;
                await Task.WhenAll(accountTask, noteTask).ConfigureAwait(false);

                _logger.Information("Data has been loaded from server.");
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}