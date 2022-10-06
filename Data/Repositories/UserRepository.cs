using Microsoft.Data.Sqlite;
using SnailPass_Desktop.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Repositories
{
    internal class UserRepository : RepositoryBase, IUserRepository
    {
        public bool AuthenticateUser(string encryptedPassword, string email)
        {
            bool isValidUser = false;

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT 'isValid' " +
                                      "FROM users " +
                                      "WHERE password=@password AND email=@email";
                command.Parameters.Add("@password", SqliteType.Text).Value = encryptedPassword;
                command.Parameters.Add("@email", SqliteType.Text).Value = email;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isValidUser = reader[0].ToString() == "isValid" ? true : false;
                    }
                }
            }
            return isValidUser;
        }

        public void Add(UserModel user)
        {
            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "REPLACE INTO users (id, username, email, hint, nonce, password)" +
                                      "VALUES (@id, @username, @email, @hint, @nonce, @password);";

                command.Parameters.Add("@id", SqliteType.Text).Value = user.ID;
                command.Parameters.Add("@username", SqliteType.Text).Value = user.Login;
                command.Parameters.Add("@email", SqliteType.Text).Value = user.Email;
                command.Parameters.Add("@password", SqliteType.Text).Value = user.Password;
                command.Parameters.Add("@hint", SqliteType.Text).Value = (object)user.Hint ?? (object)DBNull.Value;
                command.Parameters.Add("@nonce", SqliteType.Text).Value = (object)user.Nonce ?? (object)DBNull.Value;

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

        public bool IsUsernameExist(string username)
        {
            bool isExist = false;

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT 'username exist' " +
                                      "FROM users " +
                                      "WHERE username=@username;";
                command.Parameters.Add("@username", SqliteType.Text).Value = username;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isExist = reader[0].ToString() == "username exist" ? true : false;
                    }
                }
            }

            return isExist;
        }

        public bool IsEmailExist(string email)
        {
            bool isExist = false;

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT 'email exist' " +
                                      "FROM users " +
                                      "WHERE email=@email;";
                command.Parameters.Add("@email", SqliteType.Text).Value = email;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isExist = reader[0].ToString() == "email exist" ? true : false;
                    }
                }
            }

            return isExist;
        }

        public UserModel GetByEmail(string email)
        {
            UserModel user = null;

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT id, username, hint, nonce, password " +
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
                            Login = reader[1].ToString(),
                            Email = email,
                            Hint = reader[2].ToString(),
                            Nonce = reader[3].ToString(),
                            Password = reader[4].ToString()
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
                command.CommandText = "SELECT username, email, hint, nonce, password " +
                                      "FROM users " +
                                      "WHERE id=@id;";
                command.Parameters.Add("@email", SqliteType.Text).Value = id;
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            ID = id,
                            Login = reader[0].ToString(),
                            Email = reader[1].ToString(),
                            Hint = reader[2].ToString(),
                            Nonce = reader[3].ToString(),
                            Password = reader[4].ToString()
                        };
                    }
                }
            }
            return user;
        }

        public UserModel GetByUsername(string username)
        {
            UserModel user = null;

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT id, email, hint, nonce, password " +
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
                            Login = username,
                            Hint = reader[2].ToString(),
                            Nonce = reader[3].ToString(),
                            Password = reader[4].ToString()
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
