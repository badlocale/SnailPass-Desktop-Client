using Newtonsoft.Json;
using SnailPass_Desktop.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Data.API
{
    internal class RestAPI : IRestClient, IDisposable
    {
        private HttpClient _httpClient = null;
        private string _token = null;

        public string Token
        {
            get { return _token; }
            set
            {
                _token = value;
                _httpClient.DefaultRequestHeaders.Add("x-access-token", _token);
            }
        }

        public RestAPI()
        {
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
                login = user.Login,
                master_password_hash = user.Password,
                hint = user.Hint,
                email = user.Email,
            };

            HttpResponseMessage responce = await _httpClient.PostAsJsonAsync("users" ,pUser);
            Console.WriteLine(responce.StatusCode);
            return responce.StatusCode;
        }

        public async Task<HttpStatusCode> Login(string email, string password)
        {
            var pLoginData = new
            {
                master_password_hash = password,
                email = email
            };

            HttpResponseMessage responce = await _httpClient.PostAsJsonAsync("login", pLoginData); //POST???

            if (responce.IsSuccessStatusCode)
            {
                Task<string> jsonTaskString = responce.Content.ReadAsStringAsync();
                string jsonString = jsonTaskString.Result;
                var defenition = new { token = "" };
                var content = JsonConvert.DeserializeAnonymousType(jsonString, defenition);
                Token = content.token;
            }

            return responce.StatusCode;
        }

        public AccountModel GetAccount(string username)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AccountModel> GetAccounts()
        {
            throw new NotImplementedException();
        }

        public UserModel GetUser(string username)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetUsers()
        {
            throw new NotImplementedException();
        }

        public void PostAccount(AccountModel user)
        {
            throw new NotImplementedException();
        }

        public void PostUser(UserModel user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}