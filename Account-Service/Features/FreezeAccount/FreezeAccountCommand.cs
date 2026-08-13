using System;
using Account_Service.Helpers;
using MediatR;

namespace Account_Service.Features.FreezeAccount;

public record FreezeAccountCommand(
    string AccountNumber) : IRequest<Response<FreezeAccountResponse>> , ITransactionCommand;
