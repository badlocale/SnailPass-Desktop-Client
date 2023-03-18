using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnailPass.Model.Interfaces
{
    public interface IAccountRepository
    {
        void AddOrReplace(AccountModel user);
        void RepaceAll(IEnumerable<AccountModel> users, string email);
        IEnumerable<AccountModel> GetByUserId(string userId);
        void DeleteAllByUsersEmail(string email);
    }
}
