using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnailPass.Model.Interfaces
{
    public interface ICustomFieldRepository
    {
        void AddOrReplace(EncryptableFieldModel field);
        void RepaceAll(IEnumerable<EncryptableFieldModel> fields, string accountId);
        IEnumerable<EncryptableFieldModel> GetByAccountID(string userID);
        void DeleteAllByUsersEmail(string email);
    }
}
