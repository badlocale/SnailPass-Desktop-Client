using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using SnailPass_Desktop.Model.Interfaces;

namespace SnailPass_Desktop.Model.Cryptography
{
    public class Pbkdf2Encryptor : CryptographerBase, IMasterPasswordEncryptor
    {
        private const int DefaultIterationCount = 120000;
        private readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
        private const int KeySize = 32;

        public string Encrypt(SecureString password, string salt, int iterationCount = DefaultIterationCount)
        {
            string strPassword = SecureStringToString(password);
            byte[] hashedMaster = Rfc2898DeriveBytes.Pbkdf2(strPassword, Encoding.UTF8.GetBytes(salt), iterationCount, Algorithm, KeySize);

            return Convert.ToBase64String(hashedMaster);
        }
    }
}