using System;
using BankSystem.GrpcContracts.Protos.Account.v1;
using Grpc.Core;
using MediatR;

namespace Account_Service.Features.DeductBalance;

public class DeductBalanceGrpc : AccountGrpcService.AccountGrpcServiceBase
{
    private readonly IMediator mediator;

    public DeductBalanceGrpc(IMediator mediator)
    {
        this.mediator = mediator;
    }
    public override async Task<BankSystem.GrpcContracts.Protos.Account.v1.DeductBalanceResponse> DeductBalance(DeductBalanceRequest request, ServerCallContext context)
    {
        var command = new DeductBalanceCommand(request.AccountNumber, (decimal)request.Amount);

        var result = await mediator.Send(command);

        return new BankSystem.GrpcContracts.Protos.Account.v1.DeductBalanceResponse
        {
            IsSuccess = result.Succeeded,
            ErrorMessage = result.Message ?? string.Empty
        };
    }


}
