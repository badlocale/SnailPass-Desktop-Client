using SnailPass_Desktop.Services;
using System.Security;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<LoggingResult> Login(string email, SecureString password);
        public Task<RegistrationResult> Register(string email, SecureString password, string hint);
    }
}
