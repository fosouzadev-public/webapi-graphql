namespace WebApi.Types;

public class AuthorType
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Cellphone { get; init; }
    public string Email { get; init; }
    public IEnumerable<BookType> Books { get; init; }
}

public class AuthorTypeDescriptor : ObjectType<AuthorType>
{
    protected override void Configure(IObjectTypeDescriptor<AuthorType> descriptor)
    {
        descriptor.BindFieldsImplicitly();
    }
}