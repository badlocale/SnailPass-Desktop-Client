using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface IAccountRepository
    {
        void AddOrReplace(AccountModel user);
        void Remove(string id);
        AccountModel GetById(string id);
        IEnumerable<AccountModel> GetByUserID(string id);
    }
}
