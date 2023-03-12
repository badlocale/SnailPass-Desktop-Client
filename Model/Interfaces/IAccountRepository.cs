using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface IAccountRepository
    {
        void AddOrReplace(AccountModel user);
        void RepaceAll(IEnumerable<AccountModel> users);
        IEnumerable<AccountModel> GetByUserId(string userId);
        void DeleteAllByUsersEmail(string email);
    }
}
