using SnailPass.Services;
using System;
using System.Security;
using System.Threading.Tasks;

namespace SnailPass.Model.Interfaces
{
    public interface IAuthenticationService
    {
        public event EventHandler LoggedViaNetwork;
        public event EventHandler LoggedLocally;

        public Task<RegistrationResult> Register(string email, SecureString password, string hint);
        public Task<LoggingResult> Login(string email, SecureString password);
        public Task<LoggingResult> LoginViaNetwork(string email, SecureString password);
        public LoggingResult LoginLocally(string email, SecureString password);
    }
}
