using SnailPass_Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SnailPass_Desktop.Model.Interfaces;

namespace SnailPass_Desktop.Repositories
{
    public class AccountRepository : RepositoryBase, IAccountRepository
    {
        public void AddOrReplace(AccountModel account)
        {
            using var connection = GetConnection();
            using (SqliteCommand command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "REPLACE INTO accounts (id, service_name, login, " +
                                      "encrypted_password, user_id, is_favorite, is_deleted, " +
                                      "creation_time, update_time, nonce) " +
                                      "VALUES (@id, @service_name, @login, @encrypted_password, @user_id, " +
                                      "@is_favorite, @is_deleted, @creation_time, @update_time, @nonce)";

                command.Parameters.Add("@id", SqliteType.Text).Value = account.ID;
                command.Parameters.Add("@service_name", SqliteType.Text).Value = account.ServiceName;
                command.Parameters.Add("@login", SqliteType.Text).Value = (object)account.Login ?? (object)DBNull.Value;
                command.Parameters.Add("@encrypted_password", SqliteType.Text).Value = account.Password;
                command.Parameters.Add("@user_id", SqliteType.Text).Value = account.UserId;
                command.Parameters.Add("@is_favorite", SqliteType.Text).Value = (object)account.IsFavorite;
                command.Parameters.Add("@is_deleted", SqliteType.Text).Value = (object)account.IsDeleted;
                command.Parameters.Add("@creation_time", SqliteType.Text).Value = account.CreationTime;
                command.Parameters.Add("@update_time", SqliteType.Text).Value = account.UpdateTime;
                command.Parameters.Add("@nonce", SqliteType.Text).Value = account.Nonce;

                command.ExecuteNonQuery();
            }
        }

        public AccountModel GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AccountModel> GetByUserID(string userId)
        {
            List<AccountModel> accounts = new List<AccountModel>();

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT id, service_name, login, encrypted_password, " +
                                      "user_id, is_favorite, is_deleted, creation_time, " +
                                      "update_time, nonce " +
                                      "FROM accounts " +
                                      "WHERE user_id = @user_id AND is_deleted = 'false';";

                command.Parameters.Add("@user_id", SqliteType.Text).Value = userId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AccountModel account = new AccountModel()
                        {
                            ID = reader[0].ToString(),
                            ServiceName = reader[1].ToString(),
                            Login = reader[2].ToString(),
                            Password = reader[3].ToString(),
                            IsFavorite = reader[4].ToString(),
                            UserId = userId
                        };

                        accounts.Add(account);
                    }
                }
            }

            return accounts;
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }
    }
}
