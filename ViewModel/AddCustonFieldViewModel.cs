using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Stores;
using System;

namespace SnailPass_Desktop.ViewModel
{
    public class AddCustonFieldViewModel : ViewModelBase
    {
        private readonly int ENC_KEY_ITERATIONS_COUNT;

        private string _id;
        private string _fieldName;
        private string _value;
        private string _accountID;

        private IUserIdentityStore _identity;
        private IMasterPasswordEncryptor _encryptor;
        private ISymmetricCryptographer _cryptographer;
        private ILogger _logger;

        public string FieldName
        {
            get { return _fieldName; }
            set
            {
                _fieldName = value;
                OnPropertyChanged();
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public AddCustonFieldViewModel(string accountId, IUserIdentityStore identity, IMasterPasswordEncryptor encryptor, 
            ISymmetricCryptographer cryptographer, ILogger logger)
        {


            _identity = identity;
            _encryptor = encryptor;
            _cryptographer = cryptographer;
            _logger = logger;

            _id = Guid.NewGuid().ToString();
            _accountID = accountId;
        }

        public CustomFieldModel CreateModel()
        {
            CustomFieldModel model = new();

            model.ID = _id;
            model.AccountId = _accountID;
            model.FieldName = _fieldName;
            string encKey = _encryptor.Encrypt(_identity.Master, _identity.CurrentUser.Email, ENC_KEY_ITERATIONS_COUNT);
            (model.Value, model.Nonce) = _cryptographer.Encrypt(_value, encKey);

            return model;
        }
    }
}
