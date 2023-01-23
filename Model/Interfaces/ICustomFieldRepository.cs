using System.Collections.Generic;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface ICustomFieldRepository
    {
        void AddOrReplace(EncryptedFieldModel customField);
        IEnumerable<EncryptedFieldModel> GetByAccountID(string userID);
        void ResetByEmail(string email);
    }
}
