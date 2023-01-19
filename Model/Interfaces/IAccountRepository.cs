using System.Collections.Generic;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface IAccountRepository
    {
        void AddOrReplace(AccountModel user);
        IEnumerable<AccountModel> GetByUserID(string userId);
    }
}
