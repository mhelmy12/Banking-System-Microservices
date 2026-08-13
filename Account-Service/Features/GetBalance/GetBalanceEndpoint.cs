using System;
using Account_Service.Helpers;
using Carter;
using MediatR;

namespace Account_Service.Features.GetBalance;

public class GetBalanceEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/balance/{accountNumber}", async (string accountNumber, IMediator mediator) =>
        {
            var query = new GetBalanceQuery(accountNumber);
            var response = await mediator.Send(query);
            return EndpointResponse.Result(response);
        });
    }
}
