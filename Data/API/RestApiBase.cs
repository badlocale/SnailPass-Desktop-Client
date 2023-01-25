using System;
using System.Net.Http.Headers;
using System.Net.Http;
using Serilog;
using System.Configuration;
using System.Net;

namespace SnailPass_Desktop.Data.API
{
    public abstract class RestApiBase
    {
        public static event EventHandler TokenExpired;
        public static event EventHandler ServerDisconnected;

        protected static HttpClient HttpClient = null;
        private static string _token = null;

        protected HttpResponseMessage? Responce;

        protected ILogger _logger;

        public string Token
        {
            get { return _token; }
            set
            {
                _token = value;
                HttpClient.DefaultRequestHeaders.Remove("x-access-token");
                HttpClient.DefaultRequestHeaders.Add("x-access-token", _token);
            }
        }

        public HttpResponseMessage? ResponseMessage
        {
            get { return Responce; }
            set
            {
                Responce = value;
                CheckIsTokenExpired(Responce?.StatusCode);
            }
        }

        public RestApiBase(ILogger logger)
        {
            _logger = logger;

            HttpClient = new HttpClient();
            HttpClient.Timeout = TimeSpan.FromSeconds(900);
            HttpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["base_api_url"]);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Dispose()
        {
            HttpClient.Dispose();
        }

        private void OnTokenIxpired()
        {
            _logger.Information("Token expired.");
            TokenExpired?.Invoke(this, EventArgs.Empty);
        }

        protected void OnServerDisconnected()
        {
            _logger.Information("Server disconnected.");
            ServerDisconnected?.Invoke(this, EventArgs.Empty);
        }

        protected void CheckIsTokenExpired(HttpStatusCode? code)
        {
            if (code != null && code == HttpStatusCode.Unauthorized)
            {
                OnTokenIxpired();
            }
        }
    }
}
