using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface ICustomFieldRepository
    {
        void AddOrReplace(EncryptableFieldModel customField);
        Task<IEnumerable<EncryptableFieldModel>> GetByAccountID(string userID);
        void DeleteAllByEmail(string email);
    }
}
