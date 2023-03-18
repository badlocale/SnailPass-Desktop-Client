using Microsoft.Data.Sqlite;
using SnailPass.Model;
using SnailPass.Model.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnailPass.Data.Repositories
{
    public class CustomFieldRepository : RepositoryBase, ICustomFieldRepository
    {
        public void RepaceAll(IEnumerable<EncryptableFieldModel> fields, string accountId)
        {
            using SqliteConnection connection = GetConnection();
            connection.Open();
            using (SqliteTransaction transaction = connection.BeginTransaction())
            {
                using (SqliteCommand deleteCommand = connection.CreateCommand())
                {

                    deleteCommand.CommandText = "DELETE " +
                                                "FROM additional_fields " +
                                                "WHERE additional_fields.account_id = @accountId;";
                    deleteCommand.Parameters.Add("@accountId", SqliteType.Text).Value = accountId;

                    deleteCommand.ExecuteNonQuery();
                }

                if (fields.Count() > 0)
                {
                    using (SqliteCommand insertCommand = connection.CreateCommand())
                    {
                        StringBuilder sb = new();
                        sb.Append("INSERT INTO additional_fields (id, field_name, value, account_id) " +
                                  "VALUES");
                        foreach (EncryptableFieldModel field in fields)
                        {
                            sb.Append($"('{field.ID}', '{field.FieldName}', '{field.Value}', " +
                                $"'{field.AccountId}'),");
                        }
                        sb.Remove(sb.Length - 1, 1).Append(";");
                        insertCommand.CommandText = sb.ToString();

                        insertCommand.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
        }

        public void AddOrReplace(EncryptableFieldModel customField)
        {
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

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<EncryptableFieldModel> GetByAccountID(string accountId)
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

                using (SqliteDataReader reader = command.ExecuteReader())
                {
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

        public void DeleteAllByUsersEmail(string email)
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

                command.ExecuteNonQuery();
            }
        }
    }
}