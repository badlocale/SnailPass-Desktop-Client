using Serilog;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SnailPass_Desktop.Model.Cryptography
{
    public class CryptographyService : ICryptographyService
    {
        private ISymmetricCryptographer _cryptographer;
        private IMasterPasswordEncryptor _encryptor;
        private IUserIdentityStore _identity;
        private ILogger _logger;

        public CryptographyService(ISymmetricCryptographer cryptographer, IMasterPasswordEncryptor encryptor, 
            IUserIdentityStore identity, ILogger logger)
        {
            _cryptographer = cryptographer;
            _encryptor = encryptor;
            _identity = identity;
            _logger = logger;
        }

        public void Encrypt<ModelType>(ModelType model)
        {
            IEnumerable<PropertyInfo> properties = SearchForCryptableProperty<ModelType>();
            if (properties.Count() == 0)
            {
                _logger.Error("No one cryptable field was not found in the encrypted object.");
                return;
            }

            PropertyInfo nonceProperty = SearhForNonceProperty(model);

            string encKey = _encryptor.Encrypt(_identity.Master, _identity.CurrentUser.Email, CryptoConstants.ENC_KEY_ITERATIONS_COUNT);
            string encryptedValue = null;
            string? nonceStr = null;
            byte[]? nonce = null;
            foreach (PropertyInfo property in properties)
            {
                string? plaintext = property.GetValue(model) as string;

                if (string.IsNullOrEmpty(plaintext))
                {
                    continue;
                }

                (encryptedValue, nonceStr) = _cryptographer.Encrypt(plaintext, encKey, nonce);
                nonce = Convert.FromBase64String(nonceStr);
                property.SetValue(model, encryptedValue);
            }

            nonceProperty.SetValue(model, nonceStr);
        }

        public void Decrypt<ModelType>(ModelType model)
        {
            IEnumerable<PropertyInfo> properties = SearchForCryptableProperty<ModelType>();
            if (properties.Count() == 0)
            {
                _logger.Error("No one cryptable field was not found in the encrypted object.");
                return;
            }

            PropertyInfo nonceProperties = SearhForNonceProperty(model);
            string? nonce = nonceProperties.GetValue(model) as string;
            if (nonce == null)
            {
                _logger.Error("Nonce field was not found in the encrypted object.");
                return;
            }

            string encKey = _encryptor.Encrypt(_identity.Master, _identity.CurrentUser.Email, CryptoConstants.ENC_KEY_ITERATIONS_COUNT);
            foreach (PropertyInfo property in properties)
            {
                string? encValue = property.GetValue(model) as string;

                if (string.IsNullOrEmpty(encValue))
                {
                    continue;
                }

                string encryptedValue = _cryptographer.Decrypt(encValue, encKey, nonce);
                property.SetValue(model, encryptedValue);
            }
        }

        private IEnumerable<PropertyInfo> SearchForCryptableProperty<ModelType>()
        {
            Type modelType = typeof(ModelType);
            IEnumerable<PropertyInfo> properties = from f in modelType.GetProperties()
                                               where f.GetCustomAttributes(typeof(CryptableFieldAttribute), false).Length > 0 &&
                                                     f.DeclaringType == modelType
                                               select f;
            return properties;
        }

        private PropertyInfo SearhForNonceProperty<ModelType>(ModelType model)
        {
            Type modelType = typeof(ModelType);
            PropertyInfo nonceProperty = (from n in modelType.GetProperties()
                                       where n.GetCustomAttributes(typeof(NonceFieldAttribute), false).Length > 0 &&
                                             n.DeclaringType == modelType
                                       select n).First();
            return nonceProperty;
        }
    }
}
