using Microsoft.Data.Sqlite;

namespace SnailPass.Data.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;

        public RepositoryBase()
        {
            _connectionString = $"Data Source=localdata.db;";
            InitDatabase();
        }

        protected SqliteConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        private void InitDatabase()
        {
            using var connection = GetConnection();
            using SqliteCommand command = new();
            connection.Open();
            EnableWalModel(connection);
            CreateUserTable(connection);
            CreateAccountRepository(connection);
            CreateNoteTable(connection);
            CreateCustomFieldTable(connection);
        }

        private void EnableWalModel(SqliteConnection connection)
        {
            using SqliteCommand command = new();
            command.Connection = connection;
            command.ExecuteNonQuery();
        }

        private void CreateUserTable(SqliteConnection connection)
        {
            using SqliteCommand command = new();
            command.Connection = connection;
            command.CommandText = "CREATE TABLE IF NOT EXISTS users (" +
                                      "id TEXT PRIMARY KEY," +
                                      "email TEXT NOT NULL UNIQUE," +
                                      "hint TEXT," +
                                      "password varchar NOT NULL" +
                                  ");";
            command.ExecuteNonQuery();
        }

        private void CreateAccountRepository(SqliteConnection connection)
        {
            using SqliteCommand command = new();
            command.Connection = connection;
            command.CommandText = "CREATE TABLE IF NOT EXISTS accounts (" +
                                      "id TEXT PRIMARY KEY," +
                                      "service_name TEXT NOT NULL," +
                                      "login TEXT NOT NULL," +
                                      "encrypted_password TEXT NOT NULL," +
                                      "user_id TEXT NOT NULL," +
                                      "is_favorite BOOLEAN," +
                                      "is_deleted BOOLEAN," +
                                      "creation_time DATETIME NOT NULL," +
                                      "update_time DATETIME NOT NULL," +
                                      "FOREIGN KEY(user_id) REFERENCES users(id)" +
                                  ");";
            command.ExecuteNonQuery();
        }

        private void CreateNoteTable(SqliteConnection connection)
        {
            using SqliteCommand command = new();
            command.Connection = connection;
            command.CommandText = "CREATE TABLE IF NOT EXISTS notes (" +
                                      "id TEXT PRIMARY KEY," +
                                      "name TEXT NOT NULL," +
                                      "content TEXT NOT NULL," +
                                      "user_id TEXT NOT NULL," +
                                      "is_favorite BOOLEAN," +
                                      "is_deleted BOOLEAN," +
                                      "creation_time DATETIME NOT NULL," +
                                      "update_time DATETIME NOT NULL," +
                                      "FOREIGN KEY(user_id) REFERENCES users(id)" +
                                  ");";
            command.ExecuteNonQuery();
        }

        private void CreateCustomFieldTable(SqliteConnection connection)
        {
            using SqliteCommand command = new();
            command.Connection = connection;
            command.CommandText = "CREATE TABLE IF NOT EXISTS additional_fields(" +
                                      "id varchar," +
                                      "field_name varchar NOT NULL," +
                                      "value varchar NOT NULL," +
                                      "account_id," +
                                      "PRIMARY KEY(id)," +
                                      "FOREIGN KEY(account_id) REFERENCES accounts(id) ON DELETE CASCADE" +
                                  ");";
            command.ExecuteNonQuery();
        }
    }
}
