using WebApi.Models;

namespace WebApi.Infrastructure.Repositories;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAsync(bool getBooks = false);
    Task<Author> GetByIdAsync(string id, bool getBooks = false);
    Task CreateAsync(Author author);
    Task UpdateAsync(Author author);
}