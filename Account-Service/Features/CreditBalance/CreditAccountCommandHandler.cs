using System;
using Account_Service.Data;
using Account_Service.Helpers;
using Account_Service.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Features.CreditBalance;

public class CreditAccountCommandHandler(AccountDbContext dbContext) : ResponseHandler, IRequestHandler<CreditAccountCommand, Response<CreditAccountResponse>>
{
    public async Task<Response<CreditAccountResponse>> Handle(CreditAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.AccountNumber, cancellationToken);

        if (account == null)
        {
            return BadRequest<CreditAccountResponse>("Account not found");
        }

        if (account.Status == AccountStatus.Closed || account.Status == AccountStatus.Inactive)
        {
            return BadRequest<CreditAccountResponse>("Account is Inactive or Closed. Cannot credit balance.");
        }

        var rowsAffected = await dbContext.Accounts
              .Where(a => a.AccountNumber == request.AccountNumber
                  && a.Status != AccountStatus.Closed
                  && a.Status != AccountStatus.Inactive)
              .ExecuteUpdateAsync(s => s
                  .SetProperty(a => a.Balance, a => a.Balance + request.Amount),
                  cancellationToken);


        if (rowsAffected == 0)
            return BadRequest<CreditAccountResponse>("Account is Inactive or Closed. Cannot credit balance.");


        return new Response<CreditAccountResponse>
        {
            Succeeded = true,
            Message = "Balance credited successfully",
            Data = new CreditAccountResponse(account.AccountNumber, account.Balance + request.Amount)
        };
    }
}
