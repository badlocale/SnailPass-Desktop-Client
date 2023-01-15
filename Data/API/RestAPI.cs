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
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Data.API
{
    public class RestAPI : IRestClient, IDisposable
    {
        private HttpClient _httpClient = null;
        private string _token = null;

        private ILogger _logger;

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

        public async Task<HttpStatusCode> Registration(UserModel user)
        {
            var pUser = new
            {
                id = user.ID,
                master_password_hash = user.Password,
                hint = user.Hint,
                email = user.Email
            };

            HttpResponseMessage responce;
            try
            {
                responce = await _httpClient.PostAsJsonAsync("users", pUser);
            }
            catch (SocketException e)
            {
                _logger.Error($"Cant connect to the server.");
                throw new Exception("It is impossible to \"register\" because the server is unavailable", e);
            }

            _logger.Information($"Registration status: {responce.StatusCode}");

            return responce.StatusCode;
        }

        public async Task<HttpStatusCode> Login(string email, string password)
        {
            string authenticationString = $"{email}:{password}";
            var base64uthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64uthenticationString);

            HttpResponseMessage responce;
            try
            {
                responce = await _httpClient.GetAsync("login");
            }
            catch (SocketException e)
            {
                _logger.Error($"Cant connect to the server.");
                throw new Exception("It is impossible to \"log in\" because the server is unavailable", e);
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

        public async Task<IEnumerable<AccountModel>> GetAccounts()
        {
            HttpResponseMessage responce;
            try
            {
                responce = await _httpClient.GetAsync("records");
            }
            catch (SocketException e)
            {
                _logger.Error($"Cant connect to the server.");
                throw new Exception("It is impossible to \"get accounts\" because the server is unavailable", e);
            }

            IEnumerable<AccountModel> accounts = new List<AccountModel>();
            if (responce.IsSuccessStatusCode)
            {
                string jsonString = responce.Content.ReadAsStringAsync().Result;
                _logger.Debug(jsonString);
            }
            
            return accounts;
        }

        public async Task<UserModel> GetUserAsync(string email)
        {
            HttpResponseMessage responce;
            try
            {
                responce = await _httpClient.GetAsync("users");
            }
            catch (SocketException e)
            {
                _logger.Error($"Cant connect to the server.");
                throw new Exception("It is impossible to \"get user\" because the server is unavailable", e);
            }

            UserModel? user = null;
            if (responce.IsSuccessStatusCode)
            {
                string jsonString = responce.Content.ReadAsStringAsync().Result;
                var defenition = new { id = "", email = "", master_password_hash = "", is_admin = "", hint = ""};
                var content = JsonConvert.DeserializeAnonymousType(jsonString, defenition);
                user = new UserModel(content.id, content.email, content.hint, content.master_password_hash);
            }

            _logger.Information($"Getting user status: {responce.StatusCode}");

            if (user == null)
            {
                throw new NotImplementedException();
            }

            return user;
        }

        public void PostAccount(AccountModel user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}