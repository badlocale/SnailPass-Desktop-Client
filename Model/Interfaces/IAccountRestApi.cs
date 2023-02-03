using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface IAccountRestApi
    {
        public Task<(HttpStatusCode?, IEnumerable<AccountModel>?)> GetAccountsAsync();
        public Task<HttpStatusCode?> PostAccountAsync(AccountModel account);
        public Task<HttpStatusCode?> DeleteAccountAsync(string accountID);
        public Task<HttpStatusCode?> PatchAccountAsync(AccountModel account);
    }
}
