using System;

namespace Account_Service.Features.DeductBalance;

public record DeductBalanceResponse(string AccountNumber, decimal Balance);