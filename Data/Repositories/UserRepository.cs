using Microsoft.Data.Sqlite;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using System;

namespace SnailPass_Desktop.Data.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public bool AuthenticateLocally(string localKey, string email)
        {
            bool isValidUser = false;

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT 'valid' " +
                                      "FROM users " +
                                      "WHERE password=@password AND email=@email";
                command.Parameters.Add("@password", SqliteType.Text).Value = localKey;
                command.Parameters.Add("@email", SqliteType.Text).Value = email;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isValidUser = reader[0].ToString() == "valid" ? true : false;
                    }
                }
            }
            return isValidUser;
        }

        public void AddOrReplace(UserModel user)
        {
            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "REPLACE INTO users (id, email, hint, password)" +
                                      "VALUES (@id, @email, @hint, @password);";

                command.Parameters.Add("@id", SqliteType.Text).Value = user.ID;
                command.Parameters.Add("@email", SqliteType.Text).Value = user.Email;
                command.Parameters.Add("@password", SqliteType.Text).Value = user.Password;
                command.Parameters.Add("@hint", SqliteType.Text).Value = (object)user.Hint ?? (object)DBNull.Value;

                command.ExecuteNonQuery();
            }
        }

        public bool IsUsernameExist(string username)
        {
            bool isExist = false;

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT 'username exists' " +
                                      "FROM users " +
                                      "WHERE username=@username;";
                command.Parameters.Add("@username", SqliteType.Text).Value = username;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isExist = reader[0].ToString() == "username exists" ? true : false;
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
                command.CommandText = "SELECT 'email exists' " +
                                      "FROM users " +
                                      "WHERE email=@email;";
                command.Parameters.Add("@email", SqliteType.Text).Value = email;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isExist = reader[0].ToString() == "email exists" ? true : false;
                    }
                }
            }

            return isExist;
        }

        public UserModel? GetByEmail(string email)
        {
            UserModel? user = null;

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT id, hint, password " +
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
                            Email = email,
                            Hint = reader[1].ToString(),
                            Password = reader[2].ToString()
                        };
                    }
                }
            }

            return user;
        }

        public UserModel GetById(string id)
        {
            UserModel? user = null;

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT email, hint, password " +
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
                            Email = reader[0].ToString(),
                            Hint = reader[1].ToString(),
                            Password = reader[3].ToString()
                        };
                    }
                }
            }

            return user;
        }
    }
}
