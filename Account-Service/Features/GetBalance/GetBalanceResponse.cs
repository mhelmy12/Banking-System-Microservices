using System;

namespace Account_Service.Features.GetBalance;

public record GetBalanceResponse(string AccountNumber, decimal Balance);
