using System;

namespace Transaction_Service.AccountServiceClient;

public class DeductBalanceResult
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }

}
