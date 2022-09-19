using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desctop.Model
{
    internal interface IUserRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        void Add(UserModel user);
        void Update(UserModel user);
        void Remove(string id);
        UserModel GetById(string id);
        UserModel GetByName(string username);
        UserModel GetByEmail(string email);
        IEnumerable<UserModel> GetAll();
    }
}
