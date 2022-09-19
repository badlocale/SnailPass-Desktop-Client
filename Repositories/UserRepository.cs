using Microsoft.Data.Sqlite;
using SnailPass_Desctop.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desctop.Repositories
{
    internal class UserRepository : RepositoryBase, IUserRepository
    {
        public bool AuthenticateUser(NetworkCredential credential)
        {
            throw new NotImplementedException();
        }

        public void Add(UserModel user)
        {
            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO users (id, username, email, hint, nonce)" +
                                      "VALUES (@id, @username, @email, @hint, @nonce);";
                command.Parameters.Add("@id", SqliteType.Text).Value = user.ID;
                command.Parameters.Add("@username", SqliteType.Text).Value = user.Username;
                command.Parameters.Add("@email", SqliteType.Text).Value = user.Email;
                command.Parameters.Add("@hint", SqliteType.Text).Value = user.Hint;
                command.Parameters.Add("@nonce", SqliteType.Text).Value = user.Nonce;
                command.ExecuteNonQuery();
            }
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(UserModel user)
        {
            throw new NotImplementedException();
        }

        public UserModel GetByEmail(string email)
        {
            UserModel user = null;

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT id, username, hint, nonce " +
                                      "FROM users " +
                                      "WHERE email=@email;";
                command.Parameters.Add("@email", SqliteType.Text).Value = email;
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            ID = reader[0].ToString(),
                            Username = reader[1].ToString(),
                            Email = email,
                            Hint = reader[2].ToString(),
                            Nonce = reader[3].ToString(),
                        };
                    }
                }
            }

            return user;
        }

        public UserModel GetById(string id)
        {
            UserModel user = null;

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT username, email, hint, nonce " +
                                      "FROM users " +
                                      "WHERE id=@id;";
                command.Parameters.Add("@email", SqliteType.Text).Value = id;
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Username = reader[0].ToString(),
                            Email = reader[1].ToString(),
                            Hint = reader[2].ToString(),
                            Nonce = reader[3].ToString(),
                        };
                    }
                }
            }

            return user;
        }

        public UserModel GetByName(string username)
        {
            UserModel user = null;

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT id, email, hint, nonce " +
                                      "FROM users " +
                                      "WHERE username=@username;";
                command.Parameters.Add("@email", SqliteType.Text).Value = username;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            ID = reader[0].ToString(),
                            Email = reader[1].ToString(),
                            Hint = reader[2].ToString(),
                            Nonce = reader[3].ToString(),
                        };
                    }
                }
            }

            return user;
        }

        public IEnumerable<UserModel> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
