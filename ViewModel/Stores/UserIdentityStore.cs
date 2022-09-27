using SnailPass_Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Stores
{
    internal class UserIdentityStore
    {
        internal UserModel CurrentUser { get; set; }
        internal SecureString Master { get; set; }
    }
}
