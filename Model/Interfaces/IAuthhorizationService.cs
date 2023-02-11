using SnailPass_Desktop.Services;
using System.Security;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<RegistrationResult> Register(string email, SecureString password, string hint);
        public Task<LoggingResult> Login(string email, SecureString password);
        public Task<LoggingResult> LoginViaNetwork(string email, SecureString password);
        public LoggingResult LoginLocally(string email, SecureString password);
    }
}
