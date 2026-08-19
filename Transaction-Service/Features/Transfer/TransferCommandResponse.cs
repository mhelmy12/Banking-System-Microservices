using System;

namespace Transaction_Service.Features.Transfer;

public record TransferCommandResponse(
    long transactionId,
    string senderAccountNumber,
    string receiverAccountNumber,
    decimal amount,
    string? description,
    string referenceNumber,
    string? failureReason,
    DateTime createdAt,
    DateTime? completedAt);
