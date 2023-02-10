﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace SnailPass_Desktop.Data.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;

        public RepositoryBase()
        {
            _connectionString = $"Data Source=localdata.db;";
        }

        protected SqliteConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
