using System;
using Shared.Helpers;
using MediatR;

namespace Account_Service.Features.GetBalance;

public record GetBalanceQuery(string AccounNumber) : IRequest<Response<GetBalanceResponse>>;