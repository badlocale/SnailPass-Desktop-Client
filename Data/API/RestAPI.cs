using Newtonsoft.Json;
using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Data.API
{
    public class RestAPI : IRestClient, IDisposable
    {
        private ILogger _logger;

        private HttpClient _httpClient = null;
        private string _token = null;

        public string Token
        {
            get { return _token; }
            set
            {
                _token = value;
                _httpClient.DefaultRequestHeaders.Remove("x-access-token");
                _httpClient.DefaultRequestHeaders.Add("x-access-token", _token);
            }
        }

        public RestAPI(ILogger logger)
        {
            _logger = logger;

            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(900);
            _httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["base_api_url"]);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpStatusCode> LoginAsync(string email, string password)
        {
            string authenticationString = $"{email}:{password}";
            var base64uthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64uthenticationString);

            HttpResponseMessage responce;
            try
            {
                responce = await _httpClient.GetAsync("login");
            }
            catch (HttpRequestException e)
            {
                _logger.Error($"Cant connect to the server.");
                throw new HttpRequestException("It is impossible to \"log in\" because the server is unavailable", e);
            }

            _httpClient.DefaultRequestHeaders.Remove("Authorization");

            if (responce.IsSuccessStatusCode)
            {
                string jsonString = responce.Content.ReadAsStringAsync().Result;
                var defenition = new { token = "" };
                var content = JsonConvert.DeserializeAnonymousType(jsonString, defenition);
                Token = content.token;
            }

            _logger.Information($"Logging status: {responce.StatusCode}");

            return responce.StatusCode;
        }

        public async Task<(HttpStatusCode, UserModel)> GetUserAsync(string email)
        {
            HttpResponseMessage responce;
            try
            {
                responce = await _httpClient.GetAsync("users");
            }
            catch (HttpRequestException e)
            {
                _logger.Error($"Cant connect to the server.");
                throw new HttpRequestException("It is impossible to \"get user\" because the server is unavailable", e);
            }

            UserModel? user = null;
            if (responce.IsSuccessStatusCode)
            {
                string jsonString = responce.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<UserModel>(jsonString);
            }

            _logger.Information($"Getting user status: {responce.StatusCode}");

            if (user == null)
            {
                throw new NotImplementedException(); //todo
            }

            return (responce.StatusCode, user);
        }

        public async Task<HttpStatusCode> PostUserAsync(UserModel user)
        {
            HttpResponseMessage responce;
            try
            {
                string jsonUser = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");
                responce = await _httpClient.PostAsync("users", content);
            }
            catch (HttpRequestException e)
            {
                _logger.Error($"Cant connect to the server.");
                throw new HttpRequestException("It is impossible to \"register\" because the server is unavailable", e);
            }

            _logger.Information($"Registration status: {responce.StatusCode}");

            return responce.StatusCode;
        }

        public async Task<(HttpStatusCode ,IEnumerable<AccountModel>?)> GetAccountsAsync()
        {
            HttpResponseMessage responce;
            try
            {
                responce = await _httpClient.GetAsync("records");
            }
            catch (HttpRequestException e)
            {
                _logger.Error($"Cant connect to the server.");
                throw new HttpRequestException("It is impossible to \"get accounts\" because the server is unavailable", e);
            }

            IEnumerable<AccountModel>? accounts = null;
            if (responce.IsSuccessStatusCode)
            {
                string jsonString = responce.Content.ReadAsStringAsync().Result;
                accounts = JsonConvert.DeserializeObject<List<AccountModel>>(jsonString);
            }

            _logger.Information($"Getting accounts status: {responce.StatusCode}");

            if (accounts == null)
            {
                return (responce.StatusCode, new List<AccountModel>());
            }

            return (responce.StatusCode, accounts);
        }


        public async Task<HttpStatusCode> PostAccountAsync(AccountModel account)
        {
            HttpResponseMessage responce;
            try
            {
                string jsonAccount = JsonConvert.SerializeObject(account);
                StringContent content = new StringContent(jsonAccount, Encoding.UTF8, "application/json");
                responce = await _httpClient.PostAsync("records", content);
            }
            catch (HttpRequestException e)
            {
                _logger.Error($"Cant connect to the server.");
                throw new HttpRequestException("It is impossible to \"add new record\" because the server is unavailable", e);
            }

            _logger.Information($"Registration status: {responce.StatusCode}");

            return responce.StatusCode;
        }

        public async Task<(HttpStatusCode, IEnumerable<CustomFieldModel>?)> GetCustomFieldsAsync(string accountID)
        {
            HttpResponseMessage responce;
            try
            {
                responce = await _httpClient.GetAsync($"additional_field?record_id={accountID}");
            }
            catch (HttpRequestException e)
            {
                _logger.Error($"Cant connect to the server.");
                throw new HttpRequestException("It is impossible to \"get fields\" because the server is unavailable", e);
            }

            IEnumerable<CustomFieldModel>? customFields = null;
            if (responce.IsSuccessStatusCode)
            {
                string jsonString = responce.Content.ReadAsStringAsync().Result;
                customFields = JsonConvert.DeserializeObject<IEnumerable<CustomFieldModel>>(jsonString);
            }

            _logger.Information($"Getting custom field status: {responce.StatusCode}");

            return (responce.StatusCode, customFields);
        }

        public async Task<HttpStatusCode> PostCustomFieldAsync(CustomFieldModel customField)
        {
            HttpResponseMessage responce;
            try
            {
                string jsonField = JsonConvert.SerializeObject(customField);
                StringContent content = new StringContent(jsonField, Encoding.UTF8, "application/json");
                responce = await _httpClient.PostAsync("additional_field", content);
            }
            catch (HttpRequestException e)
            {
                _logger.Error($"Cant connect to the server.");
                throw new HttpRequestException("It is impossible to \"add new record\" because the server is unavailable", e);
            }

            _logger.Information($"Adding custom field status: {responce.StatusCode}");

            return responce.StatusCode;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}