using System;

namespace Account_Service.Features.CreditBalance;

public record CreditAccountResponse(string AccountNumber, decimal Balance);