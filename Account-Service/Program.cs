using System.Reflection;
using Account_Service.Data;
using IdGen.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Account_Service.Services.AccountNumberGenerator;
using Shared.Extensions;
using Account_Service.Services.AccountIdGenerator;
using Account_Service.Behaviors;
using Carter;
var builder = WebApplication.CreateBuilder(args);


#region Database Configuration
builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AccountDbConnection")));
#endregion


builder.Services.AddGrpc();

builder.Services.AddSharedInfrastructure([Assembly.GetExecutingAssembly()], (config) => { config.AddOpenBehavior(typeof(TransactionBehavior<,>)); });

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


#region  Services Container
builder.Services.AddKeyedScoped<IAccountNumberGenerator, RedisAccountNumberGenerator>("Redis");

builder.Services.AddKeyedScoped<IAccountIdGenerator, AccountIdSnowflakeGenerator>("Snowflake");

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
app.MapGrpcService<Account_Service.Grpc.v1.AccountServiceGrpcImplV1>();
app.MapCarter();
app.Run();
