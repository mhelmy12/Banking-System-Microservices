using System;
using Carter;
using MediatR;
using Shared.Helpers;

namespace Transaction_Service.Features.Transfer;

public class TransferCommandEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/transaction/transfer", async (TransferCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return EndpointResponse.Result(response);
        });
    }
}
