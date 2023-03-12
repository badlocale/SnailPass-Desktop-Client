using SnailPass_Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using SnailPass_Desktop.Model.Interfaces;
using System.Diagnostics;

namespace SnailPass_Desktop.Data.Repositories
{
    public class AccountRepository : RepositoryBase, IAccountRepository
    {
        public async void AddOrReplace(AccountModel account)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            using var connection = GetConnection();
            using (SqliteCommand command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "REPLACE INTO accounts (id, service_name, login, " +
                                      "encrypted_password, user_id, is_favorite, is_deleted, " +
                                      "creation_time, update_time) " +
                                      "VALUES (@id, @service_name, @login, @encrypted_password, @user_id, " +
                                      "@is_favorite, @is_deleted, @creation_time, @update_time)";

                command.Parameters.Add("@id", SqliteType.Text).Value = account.ID;
                command.Parameters.Add("@service_name", SqliteType.Text).Value = account.ServiceName;
                command.Parameters.Add("@login", SqliteType.Text).Value = (object)account.Login ?? (object)DBNull.Value;
                command.Parameters.Add("@encrypted_password", SqliteType.Text).Value = account.Password;
                command.Parameters.Add("@user_id", SqliteType.Text).Value = account.UserId;
                command.Parameters.Add("@is_favorite", SqliteType.Text).Value = account.IsFavorite;
                command.Parameters.Add("@is_deleted", SqliteType.Text).Value = account.IsDeleted;
                command.Parameters.Add("@creation_time", SqliteType.Text).Value = account.CreationTime;
                command.Parameters.Add("@update_time", SqliteType.Text).Value = account.UpdateTime;

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<AccountModel>> GetByUserId(string userId)
        {
            List<AccountModel> accounts = new List<AccountModel>();

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT id, service_name, login, encrypted_password, " +
                                      "is_favorite, is_deleted, creation_time, update_time " +
                                      "FROM accounts " +
                                      "WHERE user_id = @user_id AND is_deleted = 0;";

                command.Parameters.Add("@user_id", SqliteType.Text).Value = userId;

                using (Task<SqliteDataReader> task = command.ExecuteReaderAsync())
                {
                    SqliteDataReader reader = await task.ConfigureAwait(false);
                    while (reader.Read())
                    {
                        AccountModel account = new AccountModel()
                        {
                            ID = reader[0].ToString(),
                            ServiceName = reader[1].ToString(),
                            Login = reader[2].ToString(),
                            Password = reader[3].ToString(),
                            UserId = userId,
                            IsFavorite = Convert.ToBoolean(reader[4]),
                            IsDeleted = Convert.ToBoolean(reader[5]),
                            CreationTime = reader[6].ToString(),
                            UpdateTime = reader[7].ToString()
                        };

                        accounts.Add(account);
                    }
                }
            }

            return accounts;
        }

        public async void DeleteAllByUsersEmail(string usersEmail)
        {
            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE " +
                                      "FROM accounts " +
                                      "WHERE accounts.user_id IN (" +
                                          "SELECT user_id " +
                                          "FROM users " +
                                          "WHERE users.email = @email" +
                                      ");";

                command.Parameters.Add("@email", SqliteType.Text).Value = usersEmail;

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
