using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface IRestClient
    {
        public string Token { get; set; }

        public Task<HttpStatusCode> LoginAsync(string email, string password);

        public Task<UserModel> GetUserAsync(string email);
        public Task<HttpStatusCode> PostUserAsync(UserModel user);

        public Task<IEnumerable<AccountModel>?> GetAccountsAsync();
        public Task<HttpStatusCode> PostAccountAsync(AccountModel account);
    }
}
