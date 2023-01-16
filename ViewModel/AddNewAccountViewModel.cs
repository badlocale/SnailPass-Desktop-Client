using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Configuration;
using System.Security;

namespace SnailPass_Desktop.ViewModel
{
    public class AddNewAccountViewModel : ViewModelBase
    {
        private readonly int ENC_KEY_ITERATIONS_COUNT;

        private string _id;
        private string _serviceName;
        private string? _login;
        private bool _isFavorite;
        private bool _isDeleted;
        private DateTime _creationTime;
        private DateTime _updateTime;
        private string _userId;
        private SecureString _password;

        private ISymmetricCryptographer _cryptographer;
        private IMasterPasswordEncryptor _encryptor;
        private IUserIdentityStore _identity;
        private ILogger _logger;

        public string ServiceName
        {
            get { return _serviceName; }
            set 
            { 
                _serviceName = value;
                OnPropertyChanged();
            }
        }

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
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

        public AddNewAccountViewModel(ISymmetricCryptographer cryptographer, IUserIdentityStore identity,
            ILogger logger, IMasterPasswordEncryptor encryptor)
        {
            ENC_KEY_ITERATIONS_COUNT = int.Parse(ConfigurationManager.AppSettings["enc_key_hash_iterations"]);

            _cryptographer = cryptographer;
            _encryptor = encryptor;
            _identity = identity;
            _logger = logger;

            _isFavorite = false;
            _isDeleted = false;
            _creationTime = DateTime.Now;
            _updateTime = DateTime.Now;
            _id = Guid.NewGuid().ToString();
            _userId = _identity.CurrentUser.ID;
        }

        public AccountModel GetModel()
        {
            AccountModel accountModel = new AccountModel();

            accountModel.ID = _id;
            accountModel.ServiceName = _serviceName;
            accountModel.Login = _login;
            accountModel.UserId = _userId;
            accountModel.IsFavorite = _isFavorite.ToString();
            accountModel.IsDeleted = _isDeleted.ToString();
            accountModel.CreationTime = _creationTime.ToString();
            accountModel.UpdateTime = _updateTime.ToString();

            string encKey = _encryptor.Encrypt(_identity.Master, _identity.CurrentUser.Email, ENC_KEY_ITERATIONS_COUNT);
            (accountModel.Password, accountModel.Nonce) = _cryptographer.Encrypt(_password, encKey, null);

            return accountModel;
        }
    }
}
