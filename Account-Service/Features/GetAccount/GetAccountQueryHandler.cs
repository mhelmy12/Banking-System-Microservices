using System;
using Account_Service.Data;
using Shared.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Features.GetAccount;

public class GetAccountQueryHandler(AccountDbContext dbContext) : ResponseHandler, IRequestHandler<GetAccountQuery, Response<GetAccountResponse>>
{
    public async Task<Response<GetAccountResponse>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        var account = await dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.AccountNumber, cancellationToken);
        if (account == null)
        {
            return NotFound<GetAccountResponse>("Account not found.");
        }

        var response = new GetAccountResponse
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            AccountHolderName = account.AccountHolderName,
            DailyTransactionLimit = account.DailyTransactionLimit,
            Email = account.Email,
            Phone = account.PhoneNumber,
            AccountType = account.Type,
            Status = account.Status,
            CreatedAt = account.CreatedAt,
            Balance = account.Balance,

        };

        return Success(response);
    }
}
