using Serilog;
using SnailPass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass.ViewModel.Stores
{
    public class UserIdentityStore : IUserIdentityStore
    {
        public UserModel CurrentUser { get; set; } 
        public SecureString Master { get; set; }
    }
}