using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath).AddJsonFile("ocelot.json", false, true).AddEnvironmentVariables();

builder.Services.AddCors();
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseCors(builder => builder
    .WithOrigins("http://localhost:53506")
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseOcelot().Wait();

app.Run();