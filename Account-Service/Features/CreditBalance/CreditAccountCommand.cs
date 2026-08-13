using System;
using Account_Service.Helpers;
using MediatR;

namespace Account_Service.Features.CreditBalance;

public record CreditAccountCommand(string AccountNumber, decimal Amount) : IRequest<Response<CreditAccountResponse>>, ITransactionCommand;
