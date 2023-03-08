using System.Collections.Generic;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface IAccountRepository
    {
        void AddOrReplace(AccountModel user);
        IEnumerable<AccountModel> GetByUserId(string userId);
        void DeleteAllByUsersEmail(string email);
    }
}
