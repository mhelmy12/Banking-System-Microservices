using System;

namespace Transaction_Service.Services;

public interface ITransactionIdGenerator
{
    public string Generate();

}
