using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.API
{
    public class WebAPI
    {
        public static Task<HttpResponseMessage> Get(string url)
        {
            try
            {
                string apiUrl = ConfigurationManager.AppSettings["base_api_url"] + url;

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    Task<HttpResponseMessage> responce = client.GetAsync(apiUrl);
                    responce.Wait();
                    return responce;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public static Task<HttpResponseMessage> Post<T>(string url, T model)
            where T : class
        {
            string apiUrl = ConfigurationManager.AppSettings["base_api_url"] + url;
            Console.WriteLine(apiUrl);

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    Task<HttpResponseMessage> responce = client.PostAsJsonAsync(apiUrl, model);
                    responce.Wait();
                    return responce;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public static Task<HttpResponseMessage> Put<T>(string url, T model)
            where T : class
        {
            try
            {
                string apiUrl = ConfigurationManager.AppSettings["base_api_url"] + url;

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.PutAsJsonAsync(apiUrl, model);
                    response.Wait();
                    return response;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public static Task<HttpResponseMessage> Delete(string url)
        {
            try
            {
                string apiUrl = ConfigurationManager.AppSettings["base_api_url"] + url;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.DeleteAsync(apiUrl);
                    response.Wait();
                    return response;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
