using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model
{
    internal interface IRestClient
    {
        public string Token { get; set; }

        public Task<HttpStatusCode> Login(string email, string password);
        public Task<HttpStatusCode> Registration(UserModel user);

        public UserModel GetUser(string email);
        public IEnumerable<UserModel> GetUsers();
        public void PostUser(UserModel user);

        public AccountModel GetAccount(string email);
        public IEnumerable<AccountModel> GetAccounts();
        public void PostAccount(AccountModel user);
    }
}
