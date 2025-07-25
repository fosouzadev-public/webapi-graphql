using WebApi;
using WebApi.Infrastructure;
using WebApi.Infrastructure.Repositories;
using WebApi.Types;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
DatabaseInitializer.Initialize(connectionString);

builder.Services.AddSingleton<IAuthorRepository, AuthorRepository>();
builder.Services.AddSingleton<IBookRepository, BookRepository>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddType<AuthorTypeDescriptor>()
    .AddType<BookTypeDescriptor>();

var app = builder.Build();
app.MapGraphQL();

app.Run();
