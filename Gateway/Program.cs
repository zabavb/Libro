using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath).AddJsonFile("ocelot.json", false, true).AddEnvironmentVariables();
builder.Services.AddCors();
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyOrigin());
app.UseOcelot().Wait();

app.Run();