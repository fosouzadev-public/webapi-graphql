namespace WebApi.Models;

public class Book
{
    public string Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public string AuthorId { get; init; }
}