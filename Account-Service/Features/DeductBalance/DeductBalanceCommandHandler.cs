using System;
using Account_Service.Data;
using Account_Service.Helpers;
using Account_Service.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Features.DeductBalance;

public class DeductBalanceCommandHandler(AccountDbContext dbContext) : ResponseHandler, IRequestHandler<DeductBalanceCommand, Response<DeductBalanceResponse>>
{
    public async Task<Response<DeductBalanceResponse>> Handle(DeductBalanceCommand request, CancellationToken cancellationToken)
    {
        var account = await dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.AccountNumber, cancellationToken);

        if (account == null)
        {
            return BadRequest<DeductBalanceResponse>("Account not found");
        }

        if (account.Status == AccountStatus.Closed || account.Status == AccountStatus.Inactive)
        {
            return BadRequest<DeductBalanceResponse>("Account is Inactive or Closed. Cannot deduct balance.");
        }


        if (account.Balance < request.Amount)
        {
            return BadRequest<DeductBalanceResponse>("Insufficient balance");
        }

        account.Balance -= request.Amount;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Response<DeductBalanceResponse>
        {
            Succeeded = true,
            Message = "Balance deducted successfully",
            Data = new DeductBalanceResponse(account.AccountNumber, account.Balance)
        };
    }
}
