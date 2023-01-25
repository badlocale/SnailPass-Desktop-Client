using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using SnailPass_Desktop.Model.Interfaces;

namespace SnailPass_Desktop.Model.Cryptography
{
    public class Pbkdf2Encryptor : CryptographerBase, IKeyGenerator
    {
        private readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
        private const int KeySize = 32;

        public string Encrypt(SecureString plaintext, string salt, int iterationCount)
        {
            string strPlaintext = SecureStringToString(plaintext);
            byte[] generatedKey = Rfc2898DeriveBytes.Pbkdf2(strPlaintext, Encoding.UTF8.GetBytes(salt), iterationCount, Algorithm, KeySize);

            return Convert.ToBase64String(generatedKey);
        }

        public string Encrypt(string plaintext, string salt, int iterationCount)
        {
            byte[] generatedKey = Rfc2898DeriveBytes.Pbkdf2(plaintext, Encoding.UTF8.GetBytes(salt), iterationCount, Algorithm, KeySize);

            return Convert.ToBase64String(generatedKey);
        }
    }
}