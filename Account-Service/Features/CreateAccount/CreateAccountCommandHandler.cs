using System;
using Account_Service.Data;
using Account_Service.Helpers;
using Account_Service.Models;
using Account_Service.Services.AccountIdGenerator;
using Account_Service.Services.AccountNumberGenerator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Features.CreateAccount;

public class CreateAccountCommandHandler(
    AccountDbContext dbContext,
    [FromKeyedServices("Redis")] IAccountNumberGenerator accountNumberGenerator,
    [FromKeyedServices("Snowflake")] IAccountIdGenerator accountIdGenerator
    ) : ResponseHandler, IRequestHandler<CreateAccountCommand, Response<CreateAccountResponse>>
{
    public async Task<Response<CreateAccountResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {

        var existingAccount = await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.Email == request.Email || a.PhoneNumber == request.Phone, cancellationToken);

        if (existingAccount != null)
        {
            return BadRequest<CreateAccountResponse>("An account with the same email or phone number already exists.");
        }

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

        return Created(new CreateAccountResponse
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
        }, "Account created successfully");

    }
}
