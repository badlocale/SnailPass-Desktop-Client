using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface ICustomFieldRepository
    {
        void AddOrReplace(EncryptableFieldModel field);
        void RepaceAll(IEnumerable<EncryptableFieldModel> fields);
        IEnumerable<EncryptableFieldModel> GetByAccountID(string userID);
        void DeleteAllByEmail(string email);
    }
}
