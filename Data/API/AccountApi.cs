using Newtonsoft.Json;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace SnailPass_Desktop.Data.API
{
    public class AccountApi : RestApiBase, IAccountRestApi
    {
        public AccountApi(ILogger logger) : base(logger)
        {

        }

        public async Task<(HttpStatusCode?, IEnumerable<AccountModel>?)> GetAccountsAsync()
        {
            try
            {
                Responce = await HttpClient.GetAsync("records");
            }
            catch (HttpRequestException)
            {
                OnServerNotResponding();
                return (null, null);
            }

            IEnumerable<AccountModel>? accounts = null;
            if (Responce.IsSuccessStatusCode)
            {
                string jsonString = Responce.Content.ReadAsStringAsync().Result;
                accounts = JsonConvert.DeserializeObject<List<AccountModel>>(jsonString);
            }

            _logger.Information($"Getting accounts status: {Responce.StatusCode}");

            if (accounts == null)
            {
                return (Responce.StatusCode, new List<AccountModel>());
            }

            return (Responce.StatusCode, accounts);
        }

        public async Task<HttpStatusCode?> PostAccountAsync(AccountModel account)
        {
            try
            {
                string jsonAccount = JsonConvert.SerializeObject(account);
                StringContent content = new StringContent(jsonAccount, Encoding.UTF8, "application/json");
                Responce = await HttpClient.PostAsync("records", content);
            }
            catch (HttpRequestException)
            {
                OnServerNotResponding();
                return null;
            }
            _logger.Information($"Registration status: {Responce?.StatusCode}");

            return Responce.StatusCode;
        }

        public async Task<HttpStatusCode?> DeleteAccountAsync(string accountID)
        {
            try
            {
                Responce = await HttpClient.DeleteAsync($"records?id={accountID}");
            }
            catch (HttpRequestException)
            {
                OnServerNotResponding();
                return null;
            }

            _logger.Information($"Deleting account status: {Responce.StatusCode}");

            return Responce.StatusCode;
        }
    }
}
