using SnailPass_Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace SnailPass_Desktop.Repositories
{
    internal class AccountRepository : RepositoryBase, IAccountRepository
    {
        public void Add(AccountModel account)
        {
            using var connection = GetConnection();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.CommandText = "INSERT INTO accounts (id, service_name, login, " +
                                      "password, creating_date, is_favorite, user_id) " +
                                      "VALUES (@id, @service_name, @login, @password, " +
                                      "@creating_date, @is_favorite, @user_id)";

                command.Parameters.Add("@id", SqliteType.Text).Value = account.ID;
                command.Parameters.Add("@service_name", SqliteType.Text).Value = account.ServiceName;
                command.Parameters.Add("@login", SqliteType.Text).Value = (object)account.Login ?? (object)DBNull.Value;
                command.Parameters.Add("@password", SqliteType.Text).Value = account.Password;
                command.Parameters.Add("@creating_date", SqliteType.Text).Value = account.CreatingDate;
                command.Parameters.Add("@is_favorite", SqliteType.Text).Value = account.IsFavorite;
                command.Parameters.Add("@user_id", SqliteType.Text).Value = account.UserId;

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
                command.CommandText = "SELECT id, service_name, login, password, " +
                                      "creating_time, is_favorite, user_id " +
                                      "FROM accounts " +
                                      "WHERE is_deleted = 0 " +
                                      "AND user_id = @user_id;";

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
                            CreatingDate = reader[4].ToString(),
                            IsFavorite = reader[5].ToString(),
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

        public void Update(AccountModel user)
        {
            throw new NotImplementedException();
        }
    }
}
