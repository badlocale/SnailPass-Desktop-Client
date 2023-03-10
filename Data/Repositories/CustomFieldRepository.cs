using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Data.Repositories
{
    public class CustomFieldRepository : RepositoryBase, ICustomFieldRepository
    {
        public async void AddOrReplace(EncryptableFieldModel customField)
        {
            Stopwatch stopwatch = new();//
            stopwatch.Start();//

            using var connection = GetConnection();
            using (SqliteCommand command = new())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "REPLACE INTO additional_fields (id, field_name, value, " +
                                      "account_id) " +
                                      "VALUES (@id, @field_name, @value, @account_id);";

                command.Parameters.Add("@id", SqliteType.Text).Value = customField.ID;
                command.Parameters.Add("@field_name", SqliteType.Text).Value = customField.FieldName;
                command.Parameters.Add("@value", SqliteType.Text).Value = customField.Value;
                command.Parameters.Add("@account_id", SqliteType.Text).Value = customField.AccountId;

                await command.ExecuteNonQueryAsync();
            }

            stopwatch.Stop();//
            TimeSpan ts = stopwatch.Elapsed;//
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);//
            Console.WriteLine($"Fields: {elapsedTime}");//
        }

        public async Task<IEnumerable<EncryptableFieldModel>> GetByAccountID(string accountId)
        {
            List<EncryptableFieldModel> fields = new();

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT id, field_name, value, account_id " +
                                      "FROM additional_fields " +
                                      "WHERE account_id = @account_id;";

                command.Parameters.Add("@account_id", SqliteType.Text).Value = accountId;

                using (Task<SqliteDataReader> task = command.ExecuteReaderAsync())
                {
                    SqliteDataReader reader = await task.ConfigureAwait(false);
                    while (reader.Read())
                    {
                        EncryptableFieldModel model = new EncryptableFieldModel()
                        {
                            ID = reader[0].ToString(),
                            FieldName = reader[1].ToString(),
                            Value = reader[2].ToString(),
                            AccountId = reader[3].ToString()
                        };

                        fields.Add(model);
                    }
                }
            }

            return fields;
        }

        public async void DeleteAllByEmail(string email)
        {
            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE " +
                                      "FROM additional_fields " +
                                      "WHERE additional_fields.account_id IN (" +
                                          "SELECT account_id " +
                                          "FROM accounts " +
                                          "JOIN users ON accounts.user_id = users.id " +
                                          "WHERE users.id = @email" +
                                      ");";

                command.Parameters.Add("@email", SqliteType.Text).Value = email;

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}