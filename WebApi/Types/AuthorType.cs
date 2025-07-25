namespace WebApi.Types;

public class AuthorType : ObjectType<AuthorType>
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Cellphone { get; init; }
    public string Email { get; init; }
    public IEnumerable<BookType> Books { get; init; }
    
    protected override void Configure(IObjectTypeDescriptor<AuthorType> descriptor)
    {
        descriptor.BindFieldsImplicitly();
    }
}