using System.Security;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface IMasterPasswordEncryptor
    {
        public string Encrypt(SecureString password, string salt, int iterationCount);
    }
}
