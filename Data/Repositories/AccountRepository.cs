﻿using SnailPass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using SnailPass.Model.Interfaces;
using System.Diagnostics;

namespace SnailPass.Data.Repositories
{
    public class AccountRepository : RepositoryBase, IAccountRepository
    {
        public void RepaceAll(IEnumerable<AccountModel> accounts, string usersEmail)
        {
            using SqliteConnection connection = GetConnection();
            connection.Open();
            using (SqliteTransaction transaction = connection.BeginTransaction())
            {
                using (SqliteCommand deleteCommand = connection.CreateCommand())
                {
                    deleteCommand.CommandText = "DELETE " +
                                                "FROM accounts " +
                                                "WHERE accounts.user_id IN (" +
                                                    "SELECT user_id " +
                                                    "FROM users " +
                                                    "WHERE users.email = @email" +
                                                ");";
                    deleteCommand.Parameters.Add("@email", SqliteType.Text).Value = usersEmail;
                    deleteCommand.ExecuteNonQuery();
                }

                if (accounts.Count() > 0)
                {
                    using (SqliteCommand insertCommand = connection.CreateCommand())
                    {
                        StringBuilder sb = new();
                        sb.Append("INSERT INTO accounts (id, service_name, login, " +
                                  "encrypted_password, user_id, is_favorite, is_deleted, " +
                                  "creation_time, update_time) " +
                                  "VALUES");
                        foreach (AccountModel account in accounts)
                        {
                            sb.Append($"('{account.ID}', '{account.ServiceName}', '{account.Login}', " +
                                $"'{account.Password}', '{account.UserId}', '{account.IsFavorite}', " +
                                $"'{account.IsDeleted}', '{account.CreationTime}', '{account.UpdateTime}'),");
                        }
                        sb.Remove(sb.Length - 1, 1).Append(";");
                        insertCommand.CommandText = sb.ToString();

                        insertCommand.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
        }

        public void AddOrReplace(AccountModel account)
        {
            using SqliteConnection connection = GetConnection();
            using (SqliteCommand command = new())
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

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<AccountModel> GetByUserId(string userId)
        {
            List<AccountModel> accounts = new List<AccountModel>();

            using SqliteConnection connection = GetConnection();
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id, service_name, login, encrypted_password, " +
                                      "is_favorite, is_deleted, creation_time, update_time " +
                                      "FROM accounts " +
                                      "WHERE user_id = @user_id AND is_deleted = 'False';";

                command.Parameters.Add("@user_id", SqliteType.Text).Value = userId;

                using (SqliteDataReader reader = command.ExecuteReader())
                {
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

        public void DeleteAllByUsersEmail(string usersEmail)
        {
            using SqliteConnection connection = GetConnection();
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

                command.ExecuteNonQuery();
            }
        }
    }
}
