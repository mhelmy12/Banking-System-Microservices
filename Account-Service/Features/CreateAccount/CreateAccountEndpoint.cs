using System;
using Carter;
using MediatR;

namespace Account_Service.Features.CreateAccount;

public class CreateAccountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/accounts", async (CreateAccountCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return Results.Ok(response);
        });
    }
}
