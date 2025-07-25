using Dapper;
using Microsoft.Data.Sqlite;
using WebApi.Models;

namespace WebApi.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly string _connectionString;

    public BookRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        return await connection.QueryAsync<Book>("SELECT * FROM Books");
    }

    public async Task<Book> GetByIdAsync(string id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        return await connection.QueryFirstOrDefaultAsync<Book>(
            "SELECT * FROM Books WHERE Id = @Id",
            new { Id = id }
        );
    }
    
    public async Task<IEnumerable<Book>> GetByAuthorIdAsync(string authorId)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        return await connection.QueryAsync<Book>(
            "SELECT * FROM Books WHERE AuthorId = @AuthorId",
            new { AuthorId = authorId }
        );
    }

    public async Task CreateAsync(Book book)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var sql = @"INSERT INTO Books (Id, Name, Price, AuthorId) VALUES (@Id, @Name, @Price, @AuthorId)";
        await connection.ExecuteAsync(sql, book);
    }

    public async Task UpdateAsync(Book book)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var sql = @"UPDATE Books SET Name = @Name, Price = @Price, AuthorId = @AuthorId WHERE Id = @Id";
        await connection.ExecuteAsync(sql, book);
    }
}