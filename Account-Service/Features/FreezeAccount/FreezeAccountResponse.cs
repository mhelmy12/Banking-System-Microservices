using System;

namespace Account_Service.Features.FreezeAccount;

public record FreezeAccountResponse(string AccountNumber, bool IsFrozen);
