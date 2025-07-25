namespace WebApi.Models;

public class Author
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Cellphone { get; init; }
    public string Email { get; init; }
    public IEnumerable<Book> Books { get; init; }
}