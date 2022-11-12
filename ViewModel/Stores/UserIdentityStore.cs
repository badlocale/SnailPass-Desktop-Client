using Serilog;
using SnailPass_Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Stores
{
    internal class UserIdentityStore : IUserIdentityStore
    {
        public UserModel CurrentUser { get; set; } 
        public SecureString Master { get; set; }

        public UserIdentityStore()
        {
            var c = new NetworkCredential("abc", "1234");
            Master = c.SecurePassword;
            if (Master == null)
                Console.WriteLine("wetrwtrew");
            CurrentUser = new UserModel() 
            { 
                Email = "shade.of.apple@gmail.com",
                Hint = "gff",
                Login = "badlocale",
                ID = Guid.NewGuid().ToString(),
                Password = "dfghfggew3142tre24gfvfxd"
            }; //temp
        }
    }
}