using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Cryptography;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel
{
    public class AddNewAccountViewModel : ViewModelBase
    {
        private string _id;
        private string _serviceName;
        private string? _login;
        private bool _isFavourite;
        private bool _isDeleted;
        private DateTime _creationTime;
        private string _userId;
        private SecureString _password;

        private ISymmetricCryptographer _cryptographer;
        private IMasterPasswordEncryptor _encryptor;
        private IUserIdentityStore _identity;
        private ILogger _logger;

        public string Id
        {
            get { return _id; }
        }

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

        public bool IsFavourite
        {
            get { return _isFavourite; }
        }

        public string UserId
        {
            get { return _userId; }
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
            _cryptographer = cryptographer;
            _encryptor = encryptor;
            _identity = identity;
            _logger = logger;

            _isFavourite = false;
            _isDeleted = false;
            _creationTime = DateTime.Now;
            _id = Guid.NewGuid().ToString();
            _userId = "50b1fe14-6345-46ef-9b3d-477ff20a93f8";
            //_userId = identity.CurrentUser.ID;
        }

        public AccountModel GetModel()
        {
            AccountModel accountModel = new AccountModel();

            accountModel.ID = _id;
            accountModel.ServiceName = _serviceName;
            accountModel.Login = _login;
            accountModel.IsFavorite = _isFavourite.ToString();
            accountModel.IsDeleted = _isDeleted.ToString();
            accountModel.CreationTime = _creationTime.ToString();
            accountModel.UserId = _userId;

            string encKey = _encryptor.Encrypt(_identity.Master, _identity.CurrentUser.Email, 120000);
            (accountModel.Password, accountModel.Nonce) = _cryptographer.Encrypt(_password, encKey, null);

            return accountModel;
        }
    }
}
