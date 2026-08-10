using System;
using Account_Service.Helpers;
using Carter;
using MediatR;

namespace Account_Service.Features.GetAccount;

public class GetAccountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/accounts/{accountNumber}", async (string accountNumber, IMediator mediator) =>
        {
            var query = new GetAccountQuery { AccountNumber = accountNumber };
            var response = await mediator.Send(query);
            return EndpointResponse.Result(response);
        });
    }

}
