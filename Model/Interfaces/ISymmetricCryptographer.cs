using System.Security;

namespace SnailPass.Model.Interfaces
{
    public interface ISymmetricCryptographer
    {
        public (string, string) Encrypt(string plaintext, string key, byte[]? nonce = null);
        public (string, string) Encrypt(SecureString plaintext, string key, byte[]? nonce = null);
        public string Decrypt(string encryptedData, string key, string nonce);
    }
}
