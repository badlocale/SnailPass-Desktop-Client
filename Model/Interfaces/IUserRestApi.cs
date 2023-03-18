using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass.Model.Interfaces
{
    public interface IUserRestApi
    {
        public Task<HttpStatusCode?> LoginAsync(string email, string password);
        public Task<(HttpStatusCode?, UserModel?)> GetUserAsync(string email);
        public Task<HttpStatusCode?> PostUserAsync(UserModel user);
    }
}