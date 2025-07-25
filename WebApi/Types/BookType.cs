namespace WebApi.Types;

public class BookType : ObjectType<BookType>
{
    public string Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    
    protected override void Configure(IObjectTypeDescriptor<BookType> descriptor)
    {
        descriptor.BindFieldsImplicitly();
    }
}