using Newtonsoft.Json;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace SnailPass_Desktop.Data.API
{
    public class CustomFieldsApi : RestApiBase, ICustomFieldRestApi
    {
        public CustomFieldsApi(ILogger logger) : base(logger)
        {

        }

        public async Task<(HttpStatusCode?, IEnumerable<EncryptableFieldModel>?)> GetCustomFieldsAsync(string accountID)
        {
            try
            {
                Responce = await HttpClient.GetAsync($"additional_fields?id={accountID}");

                CheckIsTokenExpired(Responce?.StatusCode);
            }
            catch (HttpRequestException)
            {
                OnServerNotResponding();
                return (null, null);
            }

            IEnumerable<EncryptableFieldModel>? field = null;
            if (Responce.IsSuccessStatusCode)
            {
                string jsonString = Responce.Content.ReadAsStringAsync().Result;
                field = JsonConvert.DeserializeObject<IEnumerable<EncryptableFieldModel>>(jsonString);
            }

            _logger.Information($"Getting custom field status: {Responce.StatusCode}");

            if (field == null)
            {
                return (Responce.StatusCode, new List<EncryptableFieldModel>());
            }

            return (Responce.StatusCode, field);
        }

        public async Task<HttpStatusCode?> PostCustomFieldAsync(EncryptableFieldModel field)
        {
            try
            {
                string jsonField = JsonConvert.SerializeObject(field);
                StringContent content = new StringContent(jsonField, Encoding.UTF8, "application/json");
                Responce = await HttpClient.PostAsync("additional_fields", content);
            }
            catch (HttpRequestException)
            {
                OnServerNotResponding();
                return null;
            }

            _logger.Information($"Adding custom field status: {Responce.StatusCode}");

            return Responce.StatusCode;
        }

        public async Task<HttpStatusCode?> DeleteCustomFieldAsync(string fieldID)
        {
            try
            {
                Responce = await HttpClient.DeleteAsync($"additional_fields?id={fieldID}");
            }
            catch (HttpRequestException)
            {
                OnServerNotResponding();
                return null;
            }

            _logger.Information($"Deleting custom field status: {Responce.StatusCode}");

            return Responce.StatusCode;
        }

        public async Task<HttpStatusCode?> PatchCustomFieldAsync(EncryptableFieldModel field)
        {
            try
            {
                string jsonField = JsonConvert.SerializeObject(field);
                StringContent content = new StringContent(jsonField, Encoding.UTF8, "application/json");
                Responce = await HttpClient.PatchAsync("additional_fields", content);
            }
            catch (HttpRequestException)
            {
                OnServerNotResponding();
                return null;
            }

            _logger.Information($"Patching custom field status: {Responce.StatusCode}");

            return Responce.StatusCode;
        }
    }
}
