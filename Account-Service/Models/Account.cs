using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Account_Service.Models;

public class Account
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }
    public string AccountNumber { get; set; }

    [Required]
    public string AccountHolderName { get; set; }
    public decimal Balance { get; set; }

    public decimal DailyTransactionLimit { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string PhoneNumber { get; set; }
    public AccountStatus Status { get; set; } = AccountStatus.Active;

    [Required]
    public AccountType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }


}

public enum AccountType
{
    Checking,
    Savings,
    Credit
}

public enum AccountStatus
{
    Active,
    Inactive,
    Closed
}