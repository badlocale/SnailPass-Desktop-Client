using SnailPass.Model;
using System.Security;

namespace SnailPass.ViewModel.Stores
{
    public interface IUserIdentityStore
    {
        UserModel CurrentUser { get; set; }
        SecureString Master { get; set; }
    }
}
