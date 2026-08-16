using System;
using Shared.Helpers;
using Carter;
using MediatR;

namespace Account_Service.Features.CreditBalance;

public class CreditAccountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/accounts/credit", async (CreditAccountCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return EndpointResponse.Result(response);
        });
    }
}
