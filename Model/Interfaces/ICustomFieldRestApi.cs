using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface ICustomFieldRestApi
    {
        public Task<(HttpStatusCode?, IEnumerable<EncryptableFieldModel>?)> GetCustomFieldsAsync(string accountID);
        public Task<HttpStatusCode?> PostCustomFieldAsync(EncryptableFieldModel customField);
        public Task<HttpStatusCode?> DeleteCustomFieldAsync(string fieldID);
        public Task<HttpStatusCode?> PatchCustomFieldAsync(EncryptableFieldModel customField);
    }
}
