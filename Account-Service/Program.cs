using System.Reflection;
using Account_Service.Data;
using IdGen.DependencyInjection;
using Carter;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Account_Service.Services.AccountNumberGenerator;
using FluentValidation;
using MediatR;
using Account_Service.Behaviors;
using Shared.Helpers;
using Account_Service.Services.AccountIdGenerator;

var builder = WebApplication.CreateBuilder(args);


#region Database Configuration
builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AccountDbConnection")));
#endregion

#region MediatR Configuration
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());

    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(TransactionBehavior<,>));
});
#endregion

#region  Carter Configuration
builder.Services.AddCarter();
#endregion

#region SnowflakeId Generator Configuration
builder.Services.AddIdGen(1);
#endregion

#region Redis Configuration
string redisConnectionString = builder.Configuration.GetConnectionString("Redis")
                               ?? throw new InvalidOperationException("Redis connection string is missing.");
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(redisConnectionString, true);
    return ConnectionMultiplexer.Connect(configuration);
});
#endregion

#region FluentValidation Configuration
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
#endregion

#region  Services Container
builder.Services.AddKeyedScoped<IAccountNumberGenerator, RedisAccountNumberGenerator>("Redis");

builder.Services.AddKeyedScoped<IAccountIdGenerator, AccountIdSnowflakeGenerator>("Snowflake");

#endregion
#region Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region Exception Handling Configuration
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
#endregion

#region OpenAPI Configuration
builder.Services.AddOpenApi();
#endregion




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
   {
       options.DocumentPath = "openapi/v1.json";
   });
}

app.UseExceptionHandler();

app.MapCarter();

app.Run();
