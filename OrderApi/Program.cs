using Microsoft.EntityFrameworkCore;
using Serilog;
using OrderApi.Data;
using OrderApi.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using OrderApi.Repository;
using OrderApi.Profiles;
using StackExchange.Redis;
using OrderAPI.Repository;
using BookAPI.Data;
using BookAPI.Repositories.Interfaces;
using BookAPI.Repositories;
using BookAPI.Services.Interfaces;
using BookAPI.Services;
using Library.Common;
using BookAPI.Data.CachHelper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<S3StorageService>();

builder.Services.AddDbContext<BookDbContext>((serviceProvider, options) =>
{
    var storageService = serviceProvider.GetRequiredService<S3StorageService>();
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookDbConnection"));
});

builder.Services.AddDbContext<OrderDbContext>(options => options
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var redisConfig = config.GetSection("Redis");

    var options = new ConfigurationOptions
    {
        EndPoints = { { redisConfig["Host"]!, int.Parse(redisConfig["Port"]!) } },
        User = redisConfig["User"],
        Password = redisConfig["Password"]
    };
    return ConnectionMultiplexer.Connect(options);
});


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IDeliveryTypeRepository, DeliveryTypeRepository>();
builder.Services.AddScoped<IDeliveryTypeService, DeliveryTypeService>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OrderApi",
        Version = "v1"
    });


    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        return apiDesc.ActionDescriptor?.DisplayName?.Contains("OrderAPI") ?? false;
    });

    options.CustomSchemaIds(type => type.FullName!.Replace('+', '.'));
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine($"{AppContext.BaseDirectory}", xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:58482")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderApi");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
