using System;
using Shared.Helpers;

namespace Transaction_Service.AccountServiceClient;

public interface IAccountServiceClient
{

    Task<DeductBalanceResult> DeductBalanceAsync(string accountNumber, decimal amount, CancellationToken cancellationToken = default);

}
