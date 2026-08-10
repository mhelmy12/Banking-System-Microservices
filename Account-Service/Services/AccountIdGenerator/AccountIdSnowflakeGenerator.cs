using System;
using IdGen;
namespace Account_Service.Services.AccountIdGenerator;

public class AccountIdSnowflakeGenerator : IAccountIdGenerator
{
    private readonly IIdGenerator<long> generator;

    public AccountIdSnowflakeGenerator(IIdGenerator<long> Generator)
    {
        generator = Generator;
    }
    public string Generate(CancellationToken cancellationToken = default)
    {
        var id = generator.CreateId();
        return id.ToString();
    }
}
