using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface IRestClient
    {
        public string Token { get; set; }

        public Task<HttpStatusCode> LoginAsync(string email, string password);

        public Task<(HttpStatusCode, UserModel)> GetUserAsync(string email);
        public Task<HttpStatusCode> PostUserAsync(UserModel user);

        public Task<(HttpStatusCode, IEnumerable<AccountModel>?)> GetAccountsAsync();
        public Task<HttpStatusCode> PostAccountAsync(AccountModel account);

        public Task<(HttpStatusCode, IEnumerable<CustomFieldModel?>)> GetCustomFieldsAsync(string accountID);
        public Task<HttpStatusCode> PostCustomFieldAsync(CustomFieldModel customField);
    }
}
