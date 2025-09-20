using Microsoft.Data.Sqlite;
using System.IO;

namespace StudentsApp.Infrastructure
{
    public static class Database
    {
        private static readonly string _dbFolder =
            Path.Combine(AppContext.BaseDirectory, "data");
        private static readonly string _dbPath =
            Path.Combine(_dbFolder, "app.db");

        public static string ConnectionString => $"Data Source={_dbPath}";

        public static void EnsureCreated()
        {
            Directory.CreateDirectory(_dbFolder);
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                PRAGMA foreign_keys = ON;
                CREATE TABLE IF NOT EXISTS Students(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    RM TEXT NOT NULL,
                    CPF TEXT NOT NULL,
                    Email TEXT NOT NULL
                );
            ";
            cmd.ExecuteNonQuery();
        }
    }
}
