using System;
using Account_Service.Data;
using Account_Service.Models;
using Account_Service.Services.AccountIdGenerator;
using Account_Service.Services.AccountNumberGenerator;
using MediatR;

namespace Account_Service.Features.CreateAccount;

public class CreateAccountCommandHandler(
    AccountDbContext dbContext,
    [FromKeyedServices("Redis")] IAccountNumberGenerator accountNumberGenerator,
    [FromKeyedServices("Snowflake")] IAccountIdGenerator accountIdGenerator
    ) : IRequestHandler<CreateAccountCommand, CreateAccountResponse>
{
    public async Task<CreateAccountResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = new Account
        {
            AccountNumber = await accountNumberGenerator.GenerateAsync(request.AccountType, cancellationToken),
            Id = long.Parse(accountIdGenerator.Generate(cancellationToken)),
            AccountHolderName = request.AccountHolderName,
            Email = request.Email,
            PhoneNumber = request.Phone,
            Type = request.AccountType,
            Balance = request.InitialDeposit,
            DailyTransactionLimit = request.AccountType switch
            {
                AccountType.Savings => 100000,
                AccountType.Checking => 30000,
                AccountType.Credit => 50000,
                _ => 0
            },
            Status = AccountStatus.Active,

            CreatedAt = DateTime.UtcNow
        };

        dbContext.Accounts.Add(account);

        return new CreateAccountResponse
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            AccountType = account.Type,
            AccountHolderName = account.AccountHolderName,
            Email = account.Email,
            Phone = account.PhoneNumber,
            Balance = account.Balance,
            DailyTransactionLimit = account.DailyTransactionLimit,
            Status = account.Status,
            CreatedAt = account.CreatedAt
        };

    }
}
