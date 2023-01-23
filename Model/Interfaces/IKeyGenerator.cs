using System.Security;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface IMasterPasswordEncryptor
    {
        public string Encrypt(SecureString plaintext, string salt, int iterationCount);
    }
}
