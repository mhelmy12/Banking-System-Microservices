using System;
using Shared.Helpers;
using MediatR;
using Account_Service.Helpers;

namespace Account_Service.Features.CreditBalance;

public record CreditAccountCommand(string AccountNumber, decimal Amount) : IRequest<Response<CreditAccountResponse>>, ITransactionCommand;
