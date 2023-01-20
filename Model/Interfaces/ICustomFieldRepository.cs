using System.Collections.Generic;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface ICustomFieldRepository
    {
        void AddOrReplace(CustomFieldModel customField);
        IEnumerable<CustomFieldModel> GetByAccountID(string userID);
        void ResetByEmail(string email);
    }
}
