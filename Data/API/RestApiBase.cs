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
        public static event EventHandler ServerNotResponding;

        protected static HttpClient HttpClient = null;
        private static string? _token = null;

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

        static RestApiBase()
        {
            HttpClient = new HttpClient();
            HttpClient.Timeout = TimeSpan.FromSeconds(900);
            HttpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["base_api_url"]);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public RestApiBase(ILogger logger)
        {
            _logger = logger;
        }

        public void Dispose()
        {
            HttpClient.Dispose();
        }

        private void OnTokenIxpired()
        {
            _logger.Warning("Token expired.");
            TokenExpired?.Invoke(this, EventArgs.Empty);
        }

        protected void OnServerNotResponding()
        {
            _logger.Warning("Server is not responding.");
            ServerNotResponding?.Invoke(this, EventArgs.Empty);
        }

        protected void CheckIsTokenExpired(HttpStatusCode? code)
        {
            if (code == HttpStatusCode.Unauthorized)
            {
                OnTokenIxpired();
            }
        }
    }
}
