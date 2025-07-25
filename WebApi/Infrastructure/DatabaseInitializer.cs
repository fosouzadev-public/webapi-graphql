using Microsoft.Data.Sqlite;

namespace WebApi.Infrastructure;

public static class DatabaseInitializer
{
    public static void Initialize(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "PRAGMA foreign_keys = ON;";
        command.ExecuteNonQuery();
        
        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Authors (
                    Id TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Cellphone TEXT NOT NULL,
                    Email TEXT NOT NULL
                );
            ";
        command.ExecuteNonQuery();
        
        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Books (
                    Id TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Price REAL NOT NULL,
                    AuthorId TEXT NOT NULL,
                    FOREIGN KEY (AuthorId) REFERENCES Authors(Id)
                );
            ";
        command.ExecuteNonQuery();
        
        // inicialiar banco para testar consulta
    }
}