using Microsoft.Data.Sqlite;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using System.Collections.Generic;

namespace SnailPass_Desktop.Data.Repositories
{
    public class CustomFieldRepository : RepositoryBase, ICustomFieldRepository
    {
        public void AddOrReplace(CustomFieldModel customField)
        {
            using var connection = GetConnection();
            using (SqliteCommand command = new())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "REPLACE INTO additional_fields (id, field_name, value, " +
                                      "nonce, account_id) " +
                                      "VALUES (@id, @field_name, @value, @nonce, @account_id);";

                command.Parameters.Add("@id", SqliteType.Text).Value = customField.ID;
                command.Parameters.Add("@field_name", SqliteType.Text).Value = customField.FieldName;
                command.Parameters.Add("@value", SqliteType.Text).Value = customField.Value;
                command.Parameters.Add("@nonce", SqliteType.Text).Value = customField.Nonce;
                command.Parameters.Add("@account_id", SqliteType.Text).Value = customField.AccountId;

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<CustomFieldModel> GetByAccountID(string accountId)
        {
            List<CustomFieldModel> fields = new();

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT id, field_name, value, nonce, account_id " +
                                      "FROM additional_fields " +
                                      "WHERE account_id = @account_id;";

                command.Parameters.Add("@account_id", SqliteType.Text).Value = accountId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomFieldModel model = new CustomFieldModel()
                        {
                            ID = reader[0].ToString(),
                            FieldName = reader[1].ToString(),
                            Value = reader[2].ToString(),
                            Nonce = reader[3].ToString(),
                            AccountId = reader[4].ToString()
                        };

                        fields.Add(model);
                    }
                }
            }
            return fields;
        }
    }
}
