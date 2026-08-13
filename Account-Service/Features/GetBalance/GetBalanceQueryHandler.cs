using System;
using Account_Service.Data;
using Account_Service.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Features.GetBalance;

public class GetBalanceQueryHandler(AccountDbContext dbContext) : ResponseHandler, IRequestHandler<GetBalanceQuery, Response<GetBalanceResponse>>
{
    public async Task<Response<GetBalanceResponse>> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
    {
        var account = await dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.AccounNumber, cancellationToken);

        if (account == null)
        {
            return NotFound<GetBalanceResponse>($"Account with number {request.AccounNumber} not found.");
        }

        var response = new GetBalanceResponse(account.AccountNumber, account.Balance);
        return Success(response);
    }
}
