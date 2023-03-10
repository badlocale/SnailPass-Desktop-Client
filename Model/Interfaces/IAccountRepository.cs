using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface IAccountRepository
    {
        void AddOrReplace(AccountModel user);
        Task<IEnumerable<AccountModel>> GetByUserId(string userId);
        void DeleteAllByUsersEmail(string email);
    }
}
