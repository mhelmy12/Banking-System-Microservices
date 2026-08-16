using System;
using Shared.Helpers;
using MediatR;
using Account_Service.Helpers;

namespace Account_Service.Features.DeductBalance;

public record DeductBalanceCommand(string AccountNumber, decimal Amount) : IRequest<Response<DeductBalanceResponse>>, ITransactionCommand;