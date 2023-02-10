using Serilog;
using SnailPass_Desktop.Model.Cryptography;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SnailPass_Desktop.Services
{
    public class CryptographyService : ICryptographyService
    {
        private ISymmetricCryptographer _cryptographer;
        private IKeyGenerator _encryptor;
        private IUserIdentityStore _identity;
        private ILogger _logger;

        public CryptographyService(ISymmetricCryptographer cryptographer, IKeyGenerator encryptor,
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

            string encKey = _encryptor.Encrypt(_identity.Master, _identity.CurrentUser.Email,
                CryptoConstants.ENC_KEY_ITERATIONS_COUNT);

            foreach (PropertyInfo property in properties)
            {
                string? plaintext = property.GetValue(model) as string;
                string? ciphertext = null;
                string? nonce = null;

                if (string.IsNullOrEmpty(plaintext))
                {
                    continue;
                }

                (ciphertext, nonce) = _cryptographer.Encrypt(plaintext, encKey);
                //nonce = Convert.FromBase64String(nonceStr);
                property.SetValue(model, $"{ciphertext}:{nonce}");
            }
        }

        public void Decrypt<ModelType>(ModelType model)
        {
            IEnumerable<PropertyInfo> properties = SearchForCryptableProperty<ModelType>();
            if (properties.Count() == 0)
            {
                _logger.Error("No one cryptable field was not found in the encrypted object.");
                return;
            }

            string encKey = _encryptor.Encrypt(_identity.Master, _identity.CurrentUser.Email,
                CryptoConstants.ENC_KEY_ITERATIONS_COUNT);

            foreach (PropertyInfo property in properties)
            {
                string? value = property.GetValue(model) as string;
                string[] valueTokens = value.Split(':');

                if (valueTokens.Length > 1)
                {
                    string? ciphertext = valueTokens[0];
                    string? nonce = valueTokens[1];

                    if (string.IsNullOrEmpty(ciphertext) || string.IsNullOrEmpty(nonce))
                    {
                        continue;
                    }

                    string encryptedValue = _cryptographer.Decrypt(ciphertext, encKey, nonce);
                    property.SetValue(model, encryptedValue);
                }

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
    }
}
