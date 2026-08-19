using BankSystem.GrpcContracts.Protos.Account.v1;
using Grpc.Core;
using MediatR;
using System;
namespace Account_Service.Grpc.v1;


public class AccountServiceGrpcImplV1 : AccountGrpcService.AccountGrpcServiceBase
{
    private readonly IMediator _mediator;

    public AccountServiceGrpcImplV1(IMediator mediator)
    {
        _mediator = mediator;

    }
    public override async Task<DeductBalanceResponse> DeductBalance(DeductBalanceRequest request, ServerCallContext context)
    {
        var command = new Features.DeductBalance.DeductBalanceCommand(request.AccountNumber, (decimal)request.Amount);

        var result = await _mediator.Send(command);

        return new DeductBalanceResponse
        {
            IsSuccess = result.Succeeded,
            ErrorMessage = result.Message ?? string.Empty
        };
    }

}
