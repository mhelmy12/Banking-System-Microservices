using System;
using Carter.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;
using Transaction_Service.Data;

namespace Transaction_Service.Features.GetTransactionById;

public class GetTransactionByIdQueryHandler(TransactionDbContext dbContext) : ResponseHandler, IRequestHandler<GetTransactionByIdQuery, Response<GetTransactionByIdQueryResponse>>
{
    public async Task<Response<GetTransactionByIdQueryResponse>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await dbContext.Transactions
                .AsNoTracking()
                .Where(t => t.Id == request.TransactionId)
                .Select(t => new GetTransactionByIdQueryResponse
                (
                    t.Id,
                    t.SenderAccountNumber,
                    t.ReceiverAccountNumber,
                    t.Amount,
                    t.Type.ToString(),
                    t.CreatedAt,
                    t.CompletedAt,
                    t.Description
                ))
                .SingleOrDefaultAsync(cancellationToken);
        if (transaction == null)
        {
            return NotFound<GetTransactionByIdQueryResponse>($"Transaction with ID {request.TransactionId} not found.");
        }


        return Success(transaction);

    }
}
