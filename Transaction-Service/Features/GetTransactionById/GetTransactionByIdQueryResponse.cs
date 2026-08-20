using System;

namespace Transaction_Service.Features.GetTransactionById;

public record GetTransactionByIdQueryResponse(
long TransactionId,
string SenderAccountNumber,
string ReceiverAccountNumber,
decimal Amount,
string TransactionType,
DateTime TransactionCreatedAt,
DateTime? TransactionCompletedAt,
string Description

);
