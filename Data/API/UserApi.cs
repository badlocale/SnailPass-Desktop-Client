using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.Model;

namespace SnailPass_Desktop.Data.API
{
    public class UserApi : RestApiBase, IUserRestApi
    {
        public UserApi(ILogger logger) : base(logger)
        {

        }

        public async Task<HttpStatusCode?> LoginAsync(string email, string password)
        {
            string authenticationString = $"{email}:{password}";
            var base64uthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64uthenticationString);

            HttpResponseMessage? responce = null;
            try
            {
                responce = await HttpClient.GetAsync("login");
            }
            catch (HttpRequestException)
            {
                OnServerDisconnected();
                return null;
            }

            HttpClient.DefaultRequestHeaders.Remove("Authorization");

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

        public async Task<(HttpStatusCode?, UserModel?)> GetUserAsync(string email)
        {
            HttpResponseMessage? responce = null;
            try
            {
                responce = await HttpClient.GetAsync("users");
            }
            catch (HttpRequestException)
            {
                OnServerDisconnected();
                return (null, null);
            }

            UserModel? user = null;
            if (responce.IsSuccessStatusCode)
            {
                string jsonString = responce.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<UserModel>(jsonString);
            }

            _logger.Information($"Getting user status: {responce.StatusCode}");

            return (responce.StatusCode, user);
        }

        public async Task<HttpStatusCode?> PostUserAsync(UserModel user)
        {
            HttpResponseMessage? responce;
            try
            {
                string jsonUser = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");
                responce = await HttpClient.PostAsync("users", content);
            }
            catch (HttpRequestException)
            {
                OnServerDisconnected();
                return null;
            }

            _logger.Information($"Registration status: {responce.StatusCode}.");

            return responce.StatusCode;
        }
    }
}
