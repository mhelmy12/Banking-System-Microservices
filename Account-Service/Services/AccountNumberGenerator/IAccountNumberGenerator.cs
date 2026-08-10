using System;
using Account_Service.Models;

namespace Account_Service.Services.AccountNumberGenerator;

public interface IAccountNumberGenerator
{
    public Task<string> GenerateAsync(string accountType, CancellationToken cancellationToken = default);
}
