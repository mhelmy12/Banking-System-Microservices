using System;
using Carter;
using MediatR;
using Shared.Helpers;

namespace Transaction_Service.Features.GetTransactionById;

public class GetTransactionByIdQueryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/transactions/{transactionId:long}", async (long transactionId, IMediator mediator) =>
        {
            var query = new GetTransactionByIdQuery(transactionId);
            var result = await mediator.Send(query);

            return EndpointResponse.Result(result);
        });
    }
}
