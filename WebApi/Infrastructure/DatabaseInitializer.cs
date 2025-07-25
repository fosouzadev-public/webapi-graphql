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
        
        command.CommandText = "SELECT COUNT(*) FROM Authors";
        string jkRowlingId = Guid.NewGuid().ToString();
        string arthurId = Guid.NewGuid().ToString();
        if (Convert.ToInt64(command.ExecuteScalar()) == 0)
        {
            command.CommandText = @$"
                INSERT INTO Authors (Id, Name, Cellphone, Email)
                VALUES 
                    ('{jkRowlingId}', 'J. K. Rowling', '(11) 96655-1234', 'jkrowling@gmail.com'),
                    ('{arthurId}', 'Arthur Conan Doyle', '(11) 96655-5678', 'arthur@gmail.com');
            ";
            command.ExecuteNonQuery();
        }
        
        command.CommandText = "SELECT COUNT(*) FROM Books";
        if (Convert.ToInt64(command.ExecuteScalar()) == 0)
        {
            command.CommandText = @$"
                INSERT INTO Books (Id, Name, Price, AuthorId)
                VALUES 
                    ('{Guid.NewGuid()}', 'Harry Potter e a Pedra Filosofal', 99.90, '{jkRowlingId}'),
                    ('{Guid.NewGuid()}', 'Harry Potter e a Câmara Secreta', 110.90, '{jkRowlingId}'),
                    ('{Guid.NewGuid()}', 'Sherlock Holmes - Um estudo em vermelho', 99.90, '{arthurId}');
            ";
            command.ExecuteNonQuery();
        }
    }
}