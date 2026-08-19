using System.Reflection;
using Carter;
using IdGen.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Shared.Extensions;
using StackExchange.Redis;
using Transaction_Service.Behaviors;
using Transaction_Service.Data;
using Transaction_Service.Services;
using BankSystem.GrpcContracts.Protos.Account.v1;
using Transaction_Service.AccountServiceClient;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
#region Database Configuration
builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TransactionDbConnection")));
#endregion

#region SnowflakeId Generator Configuration
builder.Services.AddIdGen(2);
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

builder.Services.AddKeyedSingleton<ITransactionIdGenerator, TransactionSnowflakeIdGenerator>("Snowflake");
builder.Services.AddKeyedSingleton<IAccountServiceClient, GrpcAccountServiceAdapter>("AccountService");

builder.Services.AddGrpcClient<AccountGrpcService.AccountGrpcServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration.GetConnectionString("AccountService") ?? throw new InvalidOperationException("AccountService connection string is missing."));
});

builder.Services.AddSharedInfrastructure([Assembly.GetExecutingAssembly()], (config) => { config.AddOpenBehavior(typeof(TransactionBehavior<,>)); });
builder.Services.AddOpenApi();


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

app.UseHttpsRedirection();

app.UseExceptionHandler();
app.MapCarter();

app.Run();
