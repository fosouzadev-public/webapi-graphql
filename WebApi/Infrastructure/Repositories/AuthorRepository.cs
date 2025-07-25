using Dapper;
using Microsoft.Data.Sqlite;
using WebApi.Models;

namespace WebApi.Infrastructure.Repositories;

public class AuthorRepository(IConfiguration config) : IAuthorRepository
{
    private readonly string _connectionString = config.GetConnectionString("DefaultConnection");

    private const string GetAuthorsWithBooks = @"SELECT
          a.Id as AuthorId, a.Name as AuthorName, a.Cellphone as AuthorCellphone, a.Email as AuthorEmail,
          b.Id as BookId, b.Name as BookName, b.Price as BookPrice
        FROM Authors a
          INNER JOIN Books b ON a.Id = b.AuthorId";

    private class AuthorBook
    {
        public string AuthorId { get; init; }
        public string AuthorName { get; init; }
        public string AuthorCellphone { get; init; }
        public string AuthorEmail { get; init; }
        public string BookId { get; init; }
        public string BookName { get; init; }
        public decimal BookPrice { get; init; }
    };

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var authors = await connection.QueryAsync<AuthorBook>(GetAuthorsWithBooks);
            
        return authors
            .GroupBy(a => a.AuthorId)
            .Select(a => new Author
            {
                Id = a.Key,
                Name = a.First().AuthorName,
                Cellphone = a.First().AuthorCellphone,
                Email = a.First().AuthorEmail,
                Books = a.Select(b => new Book
                {
                    Id = b.BookId,
                    Name = b.BookName,
                    Price = b.BookPrice,
                    AuthorId = b.AuthorId
                })
            });
    }

    public async Task<Author> GetByIdOrEmailAsync(string id = null, string email = null)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        bool hasId = string.IsNullOrWhiteSpace(id) is false;
        
        string field = hasId ? "a.Id" : "a.Email";
        string filter = hasId ? id : email;

        return (await connection.QueryAsync<AuthorBook>(
                $"{GetAuthorsWithBooks} WHERE {field} = @Filter",
                new { Filter = filter }))
            .GroupBy(a => a.AuthorId)
            .Select(a => new Author
            {
                Id = a.Key,
                Name = a.First().AuthorName,
                Cellphone = a.First().AuthorCellphone,
                Email = a.First().AuthorEmail,
                Books = a.Select(b => new Book
                {
                    Id = b.BookId,
                    Name = b.BookName,
                    Price = b.BookPrice,
                    AuthorId = b.AuthorId
                })
            })
            .SingleOrDefault();
    }

    public async Task CreateAsync(Author author)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var sql = @"INSERT INTO Authors (Id, Name, Cellphone, Email) VALUES (@Id, @Name, @Cellphone, @Email)";
        await connection.ExecuteAsync(sql, author);
    }

    public async Task UpdateAsync(Author author)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var sql = @"UPDATE Authors SET Name = @Name, Cellphone = @Cellphone, Email = @Email WHERE Id = @Id";
        await connection.ExecuteAsync(sql, author);
    }
}