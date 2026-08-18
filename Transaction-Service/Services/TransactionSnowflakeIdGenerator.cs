using System;
using IdGen;

namespace Transaction_Service.Services;

public class TransactionSnowflakeIdGenerator : ITransactionIdGenerator
{
    private readonly IIdGenerator<long> generator;

    public TransactionSnowflakeIdGenerator(IIdGenerator<long> Generator)
    {
        generator = Generator;
    }
    public string Generate()
    {
        var id = generator.CreateId();
        return id.ToString();
    }
}
