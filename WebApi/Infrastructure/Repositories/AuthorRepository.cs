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

    private record AuthorBook(
        string AuthorId,
        string AuthorName,
        string AuthorCellphone,
        string AuthorEmail,
        string BookId,
        string BookName,
        decimal BookPrice
    );

    public async Task<IEnumerable<Author>> GetAllAsync(bool getBooks = false)
    {
        using var connection = new SqliteConnection(_connectionString);

        if (getBooks is false)
            return await connection.QueryAsync<Author>("SELECT * FROM Authors");

        return (await connection.QueryAsync<AuthorBook>(GetAuthorsWithBooks))
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

    public async Task<Author> GetByIdAsync(string id, bool getBooks = false)
    {
        using var connection = new SqliteConnection(_connectionString);

        if (getBooks is false)
            return await connection.QueryFirstOrDefaultAsync<Author>(
                "SELECT * FROM Authors WHERE Id = @Id",
                new { Id = id }
            );
        
        var authorBook = await connection.QueryFirstOrDefaultAsync<AuthorBook>(
            $"{GetAuthorsWithBooks} WHERE a.Id = @Id",
            new { Id = id }
        );
        
        return (await connection.QueryAsync<AuthorBook>(
                $"{GetAuthorsWithBooks} WHERE a.Id = @Id",
                new { Id = id }))
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
        var sql = @"INSERT INTO Authors (Id, Name, Cellphone, Email) VALUES (@Id, @Name, @Cellphone, @Email)";
        await connection.ExecuteAsync(sql, author);
    }

    public async Task UpdateAsync(Author author)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"UPDATE Authors SET Name = @Name, Cellphone = @Cellphone, Email = @Email WHERE Id = @Id";
        await connection.ExecuteAsync(sql, author);
    }
}