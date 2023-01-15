using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace SnailPass_Desktop.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;

        public RepositoryBase()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Repositories", "localdata.db"); //TODO ref path

            _connectionString = $"Data Source=C:\\Users\\iZelton\\source\\repos\\SnailPass_Desctop\\SnailPass_Desctop\\Repositories\\localdata.db;";
        }

        protected SqliteConnection GetConnection()
        {
            return new SqliteConnection($"Data Source = C:\\Users\\iZelton\\source\\repos\\SnailPass_Desctop\\SnailPass_Desctop\\Data\\Repositories\\localdata.db");
        }
    }
}
