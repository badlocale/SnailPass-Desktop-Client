using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model
{
    internal interface IPasswordRepository
    {
        void Add(PasswordModel user);
        void Update(PasswordModel user);
        void Remove(string id);
        PasswordModel GetById(string id);
        PasswordModel GetByName(string username);
        IEnumerable<PasswordModel> GetAll();
    }
}
