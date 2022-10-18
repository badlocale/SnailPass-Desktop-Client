using SnailPass_Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Stores
{
    internal class UserIdentityStore : IUserIdentityStore
    {
        public UserModel CurrentUser { get; set; }
        public SecureString Master { get; set; }
    }
}
