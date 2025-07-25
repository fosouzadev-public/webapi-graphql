using WebApi.Models;

namespace WebApi.Infrastructure.Repositories;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author> GetByIdOrEmailAsync(string id = null, string email = null);
    Task CreateAsync(Author author);
    Task UpdateAsync(Author author);
}