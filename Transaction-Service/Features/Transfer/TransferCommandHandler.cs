using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;
using Transaction_Service.Data;
using Transaction_Service.Events;
using Transaction_Service.Models;
using Transaction_Service.Services;
using BankSystem.GrpcContracts.Protos.Account.v1;
using Transaction_Service.AccountServiceClient;

namespace Transaction_Service.Features.Transfer;

public class TransferCommandHandler : ResponseHandler, IRequestHandler<TransferCommand, Response<TransferCommandResponse>>
{
    private readonly TransactionDbContext dbContext;
    private readonly ITransactionIdGenerator transactionIdGenerator;
    private readonly IAccountServiceClient _accountClient;
    public TransferCommandHandler(
        TransactionDbContext dbContext,
        [FromKeyedServices("Snowflake")] ITransactionIdGenerator transactionIdGenerator,
        [FromKeyedServices("AccountService")] IAccountServiceClient accountClient
        )
    {
        this.dbContext = dbContext;
        this.transactionIdGenerator = transactionIdGenerator;
        this._accountClient = accountClient;
    }
    public async Task<Response<TransferCommandResponse>> Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        var existingTransaction = await dbContext.Transactions
            .FirstOrDefaultAsync(t => t.IdempotencyKey == request.IdempotencyKey, cancellationToken);

        if (existingTransaction != null)
        {
            return Success(
                new TransferCommandResponse(
                existingTransaction.Id,
                existingTransaction.SenderAccountNumber,
                existingTransaction.ReceiverAccountNumber,
                existingTransaction.Amount,
                existingTransaction.Description,
                existingTransaction.ReferenceNumber!,
                existingTransaction.FailureReason,
                existingTransaction.CreatedAt,
                existingTransaction.CompletedAt
            ));
        }

        var newTransactionId = long.Parse(transactionIdGenerator.Generate());
        var transaction = new Transaction
        {
            Id = newTransactionId,
            SenderAccountNumber = request.SenderAccountNumber,
            ReceiverAccountNumber = request.ReceiverAccountNumber,
            Amount = request.Amount,
            Type = TransactionType.TRANSFER,
            Status = TransactionStatus.PENDING,
            Description = request.Description,
            IdempotencyKey = request.IdempotencyKey,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Transactions.Add(transaction);
        await dbContext.SaveChangesAsync(cancellationToken);


        //SAGA Step 1: Deduct amount from sender's account.
        var deductResult = await _accountClient.DeductBalanceAsync(
            request.SenderAccountNumber,
            request.Amount,
            cancellationToken);

        if (!deductResult.IsSuccess)
        {
            //Transaction failed, update the status and return failure response
            transaction.Status = TransactionStatus.FAILED;
            transaction.FailureReason = deductResult.ErrorMessage; // "Insufficient Funds"

            dbContext.Transactions.Update(transaction);
            await dbContext.SaveChangesAsync(cancellationToken);

            return BadRequest<TransferCommandResponse>($"Transfer failed: {deductResult.ErrorMessage}");
        }

        //5. Update the transaction status to PROCESSING
        transaction.Status = TransactionStatus.PROCESSING;
        dbContext.Transactions.Update(transaction);

        // 6. SAGA STEP 2 (Outbox Pattern)  
        var initiatedEvent = new TransactionInitiatedEvent()
        {
            TransactionId = transaction.Id,
            SenderAccountNumber = transaction.SenderAccountNumber,
            ReceiverAccountNumber = transaction.ReceiverAccountNumber,
            Amount = transaction.Amount,
            Description = transaction.Description
        };


        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid().ToString(),
            EventType = nameof(TransactionInitiatedEvent),
            Payload = System.Text.Json.JsonSerializer.Serialize(initiatedEvent),
            OccurredOn = DateTime.UtcNow,
            AggregateId = transaction.Id.ToString(),
            AggregateType = nameof(Transaction),
        };

        dbContext.OutboxMessages.Add(outboxMessage);

        await dbContext.SaveChangesAsync(cancellationToken);


        return Success(
               new TransferCommandResponse(
               newTransactionId,
               transaction.SenderAccountNumber,
               transaction.ReceiverAccountNumber,
               transaction.Amount,
               transaction.Description,
               transaction.ReferenceNumber!,
               transaction.FailureReason,
               transaction.CreatedAt,
               transaction.CompletedAt
           ));




    }
}
