using APIComposer.GraphQL;
using APIComposer.GraphQL.Queries;
using APIComposer.GraphQL.Services;
using APIComposer.GraphQL.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Profiles));

var gatewayAddress = new Uri("https://localhost:7102/gateway/");
builder.Services.AddHttpClient<IUserServiceClient, UserServiceClient>(
    client => { client.BaseAddress = gatewayAddress; });
builder.Services.AddHttpClient<IBookServiceClient, BookServiceClient>(
    client => { client.BaseAddress = gatewayAddress; });
builder.Services.AddHttpClient<IOrderServiceClient, OrderServiceClient>(
    client => { client.BaseAddress = gatewayAddress; });

builder.Services
    .AddHttpClient()
    .AddGraphQLServer()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
    .AddQueryType<UserQuery>();
    // .AddTypeExtension<OrderQuery>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGraphQL("/graphql");

app.UseAuthorization();

app.MapControllers();

app.Run();