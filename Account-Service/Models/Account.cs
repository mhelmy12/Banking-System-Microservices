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
    public string Status { get; set; } = AccountStatus.Active;

    [Required]
    public string Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }


}

public static class AccountType
{
    public const string Checking = "Checking";
    public const string Savings = "Savings";
    public const string Credit = "Credit";
}

public static class AccountStatus
{
    public const string Active = "Active";
    public const string Inactive = "Inactive";
    public const string Closed = "Closed";
}