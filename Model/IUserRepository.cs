using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model
{
    internal interface IUserRepository
    {
        bool AuthenticateUser(string encryptedPassword, string username);
        void Add(UserModel user);
        void Update(UserModel user);
        void Remove(string id);
        bool IsUsernameExist(string username);
        bool IsEmailExist(string email);
        UserModel GetById(string id);
        UserModel GetByUsername(string username);
        UserModel GetByEmail(string email);
        IEnumerable<UserModel> GetAll();
    }
}
