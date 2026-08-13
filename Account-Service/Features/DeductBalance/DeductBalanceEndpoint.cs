using System;
using Account_Service.Helpers;
using Carter;
using MediatR;

namespace Account_Service.Features.DeductBalance;

public class DeductBalanceEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/deduct-balance", async (DeductBalanceCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return EndpointResponse.Result(response);
        });
    }
}
