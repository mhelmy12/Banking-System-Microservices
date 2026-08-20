using System;
using MediatR;
using Shared.Helpers;

namespace Transaction_Service.Features.GetTransactionById;

public record GetTransactionByIdQuery(long TransactionId) : IRequest<Response<GetTransactionByIdQueryResponse>>;
