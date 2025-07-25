namespace WebApi.Types;

public class BookType
{
    public string Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
}

public class BookTypeDescriptor : ObjectType<BookType>
{
    protected override void Configure(IObjectTypeDescriptor<BookType> descriptor)
    {
        descriptor.BindFieldsImplicitly();
    }
}