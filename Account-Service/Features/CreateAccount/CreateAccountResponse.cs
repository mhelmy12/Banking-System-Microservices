using System;

namespace Account_Service.Features.CreateAccount;

public class CreateAccountResponse
{

    public long Id { get; set; }
    public string AccountNumber { get; set; }
    public string AccountHolderName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string AccountType { get; set; }
    public string Status { get; set; }
    public decimal Balance { get; set; }
    public decimal DailyTransactionLimit { get; set; }
    public DateTime CreatedAt { get; set; }


}
