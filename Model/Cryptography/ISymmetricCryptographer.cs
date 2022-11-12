using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Cryptography
{
    internal interface ISymmetricCryptographer
    {
        public (string, string) Encrypt(SecureString password, SecureString master, byte[]? nonce = null);

        public string Decrypt(string encryptedData, string key, byte[] nonce);
    }
}
