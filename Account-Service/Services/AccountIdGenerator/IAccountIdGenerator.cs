using System;

namespace Account_Service.Services.AccountIdGenerator;

public interface IAccountIdGenerator
{

    public string Generate(CancellationToken cancellationToken = default);

}
