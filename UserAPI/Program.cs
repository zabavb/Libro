using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;
using System.Reflection;
using UserAPI.Data;
using UserAPI.Profiles;
using UserAPI.Repositories;
using UserAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "UserAPI_";
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse("localhost:6379", true);
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddAutoMapper(typeof(UserProfile));
//builder.Services.AddAutoMapper(typeof(SubscriptionProfile));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "UserAPI",
        Version = "v1"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine($"{AppContext.BaseDirectory}", xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var logFilePath = Path.Combine(AppContext.BaseDirectory, "logs.log");
Directory.CreateDirectory(Path.GetDirectoryName(logFilePath)!);

Log.Logger = new LoggerConfiguration()
    .Enrich.WithProperty("LogTime", DateTime.UtcNow)
    .WriteTo.Console(outputTemplate: "[{Level:u3}]: {Message:lj} - {LogTime:yyyy-MM-dd HH:mm:ss}{NewLine}{NewLine}")
    .WriteTo.File(
        logFilePath,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate: "[{Level:u3}]: {Message:lj} | Exception: {Exception} - {Timestamp:yyyy-MM-dd HH:mm:ss}{NewLine}{NewLine}"
    )
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "UserAPI");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
