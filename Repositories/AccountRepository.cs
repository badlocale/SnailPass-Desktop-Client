using SnailPass_Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Repositories
{
    internal class AccountRepository : RepositoryBase, IAccountRepository
    {
        public void Add(AccountModel user)
        {
            throw new NotImplementedException();
        }

        public AccountModel GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AccountModel> GetByUsername()
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(AccountModel user)
        {
            throw new NotImplementedException();
        }
    }
}
