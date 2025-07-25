using WebApi.Infrastructure.Repositories;
using WebApi.Models;
using WebApi.Types;

namespace WebApi;

public class Query(IAuthorRepository authorRepository, IBookRepository bookRepository)
{
    private static BookType BookToBookType(Book book) =>
        new BookType
        {
            Id = book.Id,
            Name = book.Name,
            Price = book.Price
        };

    private static AuthorType AuthorToAuthorType(Author author) =>
        new AuthorType
        {
            Id = author.Id,
            Name = author.Name,
            Cellphone = author.Cellphone,
            Email = author.Email,
            Books = author.Books.ToList().ConvertAll(BookToBookType)
        };
    
    public async Task<IEnumerable<AuthorType>> GetAllAuthorsAsync()
    {
        var authors = await authorRepository.GetAllAsync(true);

        return authors.Select(AuthorToAuthorType);
    }
    
    public async Task<AuthorType> GetAuthorByIdAsync(string id)
    {
        var author = await authorRepository.GetByIdAsync(id,true);

        if (author is null)
            return null;
        
        return AuthorToAuthorType(author);
    }
    
    public async Task<IEnumerable<BookType>> GetAllBooksByAuthorIdAsync(string authorId)
    {
        var books = await bookRepository.GetByAuthorIdAsync(authorId);

        return books.Select(BookToBookType);
    }

    public async Task<BookType> GetBookByIdAsync(string id)
    {
        var book = await bookRepository.GetByIdAsync(id);

        if (book is null)
            return null;

        return BookToBookType(book);
    }
}