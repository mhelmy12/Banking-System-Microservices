using System;
using Account_Service.Data;
using Shared.Helpers;
using Account_Service.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Features.FreezeAccount;

public class FreezeAccountCommandHandler(AccountDbContext dbContext) : ResponseHandler, IRequestHandler<FreezeAccountCommand, Response<FreezeAccountResponse>>
{
    public async Task<Response<FreezeAccountResponse>> Handle(FreezeAccountCommand request, CancellationToken cancellationToken)
    {

        var account = await dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.AccountNumber, cancellationToken);

        if (account == null)
        {
            return NotFound<FreezeAccountResponse>($"Account with number {request.AccountNumber} not found.");
        }

        if (account.Status == AccountStatus.Closed)
        {
            return BadRequest<FreezeAccountResponse>($"Account with number {request.AccountNumber} is already frozen.");
        }

        account.Status = AccountStatus.Closed;
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new FreezeAccountResponse(account.AccountNumber, account.Status == AccountStatus.Closed ? true : false);
        return Success(response);





    }
}
