using Microsoft.Data.Sqlite;
using SnailPass_Desctop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desctop.Repositories
{
    internal class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(UserModel user)
        {
            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO users (id, username, email, pass, hint, nonce)" +
                                      "VALUES (@id, @username, @email, @pass, @hint, @nonce);";
                command.Parameters.Add("@id", SqliteType.Text).Value = user.ID;
                command.Parameters.Add("@username", SqliteType.Text).Value = user.Username;
                command.Parameters.Add("@email", SqliteType.Text).Value = user.Email;
                command.Parameters.Add("@pass", SqliteType.Text).Value = user.Password;
                command.Parameters.Add("@hint", SqliteType.Text).Value = user.Hint;
                command.Parameters.Add("@nonce", SqliteType.Text).Value = user.Nonce;
                command.ExecuteNonQuery();
            }
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserModel GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public UserModel GetById(string id)
        {
            throw new NotImplementedException();
        }

        public UserModel GetByName(string username)
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(UserModel user)
        {
            throw new NotImplementedException();
        }
    }
}
