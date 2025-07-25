using HotChocolate.Language;
using HotChocolate.Resolvers;
using WebApi.Infrastructure.Repositories;
using WebApi.Models;
using WebApi.Types;

namespace WebApi;

public class Query(
    IAuthorRepository authorRepository,
    IBookRepository bookRepository)
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

    private bool HasField(IResolverContext context, string root, string field) =>
        context.Selection.SyntaxNodes
            .Any(node => node.Name.Value == root && node.SelectionSet.Selections
                .Any(subNode => ((FieldNode)subNode).Name.Value == field));
    
    [GraphQLName("authors")]
    public async Task<IEnumerable<AuthorType>> GetAllAuthorsAsync(IResolverContext context)
    {
        bool hasBooks = context.IsSelected(nameof(AuthorType.Books).ToLower());
        //bool hasBooks = HasField(context, "authors", nameof(AuthorType.Books).ToLower());
        
        // TODO: repassar variável para melhorar consulta no repositório
        
        var authors = await authorRepository.GetAllAsync();

        return authors.Select(AuthorToAuthorType);
    }
    
    [GraphQLName("author")]
    public async Task<AuthorType> GetAuthorByIdOrEmailAsync(string id = null, string email = null)
    {
        var author = await authorRepository.GetByIdOrEmailAsync(id, email);

        if (author is null)
            return null;
        
        return AuthorToAuthorType(author);
    }
    
    [GraphQLName("books")]
    public async Task<IEnumerable<BookType>> GetAllBooksByAuthorIdAsync(string authorId)
    {
        var books = await bookRepository.GetByAuthorIdAsync(authorId);

        return books.Select(BookToBookType);
    }

    [GraphQLName("book")]
    public async Task<BookType> GetBookByIdAsync(string id)
    {
        var book = await bookRepository.GetByIdAsync(id);

        if (book is null)
            return null;

        return BookToBookType(book);
    }
}