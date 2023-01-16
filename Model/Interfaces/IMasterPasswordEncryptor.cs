using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface IMasterPasswordEncryptor
    {
        public string Encrypt(SecureString password, string salt, int iterationCount);
    }
}
