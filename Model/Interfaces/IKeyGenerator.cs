using System.Security;

namespace SnailPass.Model.Interfaces
{
    public interface IKeyGenerator
    {
        public string Encrypt(SecureString plaintext, string salt, int iterationCount);
    }
}
