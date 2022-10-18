using SnailPass_Desktop.Model;
using System.Security;

namespace SnailPass_Desktop.ViewModel.Stores
{
    public interface IUserIdentityStore
    {
        UserModel CurrentUser { get; set; }
        SecureString Master { get; set; }
    }
}
