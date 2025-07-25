using WebApi.Models;

namespace WebApi.Infrastructure.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book> GetByIdAsync(string id);
    Task<IEnumerable<Book>> GetByAuthorIdAsync(string authorId);
    Task CreateAsync(Book book);
    Task UpdateAsync(Book book);
}