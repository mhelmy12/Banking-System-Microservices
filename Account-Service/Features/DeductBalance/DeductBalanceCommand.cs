using System;
using Account_Service.Helpers;
using MediatR;

namespace Account_Service.Features.DeductBalance;

public record DeductBalanceCommand(string AccountNumber, decimal Amount) : IRequest<Response<DeductBalanceResponse>>, ITransactionCommand;