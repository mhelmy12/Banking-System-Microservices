using System;
using Shared.Helpers;
using MediatR;
using Account_Service.Helpers;

namespace Account_Service.Features.FreezeAccount;


public record FreezeAccountCommand(
    string AccountNumber) : IRequest<Response<FreezeAccountResponse>>, ITransactionCommand;
