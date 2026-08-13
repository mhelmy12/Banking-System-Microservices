using System;
using Account_Service.Helpers;
using Carter;
using MediatR;

namespace Account_Service.Features.FreezeAccount;

public class FreezeAccountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/accounts/freeze", async (FreezeAccountCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return EndpointResponse.Result(response);
        })
        .WithName("FreezeAccount")
        .WithTags("Accounts");
    }
}
