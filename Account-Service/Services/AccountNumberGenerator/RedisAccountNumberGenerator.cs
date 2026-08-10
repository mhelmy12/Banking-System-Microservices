using System;
using Account_Service.Helpers;
using Account_Service.Models;
using StackExchange.Redis;


namespace Account_Service.Services.AccountNumberGenerator;

public class RedisAccountNumberGenerator : IAccountNumberGenerator
{
    private readonly IDatabase _redis;
    private const string SequenceKey = "Account_Sequence_Number";
    private const long InitialSeedValue = 1000000;
    public RedisAccountNumberGenerator(IConnectionMultiplexer redisConnection)
    {
        _redis = redisConnection.GetDatabase();
    }
    public async Task<string> GenerateAsync(string accountType, CancellationToken cancellationToken = default)
    {
        string prefix = accountType switch
        {
            AccountType.Checking => "10",
            AccountType.Savings => "20",
            AccountType.Credit => "30",
            _ => "99"
        };

        await _redis.StringSetAsync(SequenceKey, InitialSeedValue, null, When.NotExists);
        long sequenceValue = await _redis.StringIncrementAsync(SequenceKey);
        string baseNumber = $"{prefix}{sequenceValue}";
        int checkDigit = LuhnAlgorithm.CalculateCheckDigit(baseNumber);
        return $"{baseNumber}{checkDigit}";
    }
}
