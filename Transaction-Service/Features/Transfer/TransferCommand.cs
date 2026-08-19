using System;
using MediatR;
using Shared.Helpers;
using Transaction_Service.Helpers;

namespace Transaction_Service.Features.Transfer;

public record TransferCommand(
    string SenderAccountNumber,
    string ReceiverAccountNumber,
    decimal Amount,
    string? Description,
    string IdempotencyKey) : IRequest<Response<TransferCommandResponse>>, ITransactionCommand;
