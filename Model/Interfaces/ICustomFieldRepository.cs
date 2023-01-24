using System.Collections.Generic;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface ICustomFieldRepository
    {
        void AddOrReplace(EncryptableFieldModel customField);
        IEnumerable<EncryptableFieldModel> GetByAccountID(string userID);
        void DeleteAllByEmail(string email);
    }
}
