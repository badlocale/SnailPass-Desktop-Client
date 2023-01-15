using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface IUserRepository
    {
        bool AuthenticateLocally(string localKey, string username);
        void AddOrReplace(UserModel user);
        bool IsUsernameExist(string username);
        bool IsEmailExist(string email);
        UserModel GetById(string id);
        UserModel GetByEmail(string email);
    }
}
