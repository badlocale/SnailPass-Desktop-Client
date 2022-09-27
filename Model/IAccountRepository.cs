using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model
{
    internal interface IAccountRepository
    {
        void Add(AccountModel user);
        void Update(AccountModel user);
        void Remove(string id);
        AccountModel GetById(string id);
        IEnumerable<AccountModel> GetByUserID(string id);
    }
}
