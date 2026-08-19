using System;
using BankSystem.GrpcContracts.Protos.Account.v1;
using Grpc.Core;
using Shared.Helpers;

namespace Transaction_Service.AccountServiceClient;

public class GrpcAccountServiceAdapter : ResponseHandler, IAccountServiceClient
{
    private readonly AccountGrpcService.AccountGrpcServiceClient _grpcClient;

    public GrpcAccountServiceAdapter(AccountGrpcService.AccountGrpcServiceClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    public async Task<DeductBalanceResult> DeductBalanceAsync(string accountNumber, decimal amount, CancellationToken cancellationToken = default)
    {
        var grpcRequest = new DeductBalanceRequest
        {
            AccountNumber = accountNumber,
            Amount = (double)amount
        };

        try
        {
            // 2. Call the gRPC Service
            var grpcResponse = await _grpcClient.DeductBalanceAsync(grpcRequest, cancellationToken: cancellationToken);

            // 3. Mapping: من gRPC Response لـ Business Result
            if (grpcResponse.IsSuccess)
            {
                return new DeductBalanceResult { IsSuccess = true };
            }

            return new DeductBalanceResult { IsSuccess = false, ErrorMessage = grpcResponse.ErrorMessage };
        }
        catch (RpcException ex)
        {
            return new DeductBalanceResult { IsSuccess = false, ErrorMessage = $"Communication error with Account Service: {ex.Status.Detail}" };
        }

    }
}