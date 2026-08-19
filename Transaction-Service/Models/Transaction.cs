using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Transaction_Service.Models;

public class Transaction
{


    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }


    [Required]
    public string IdempotencyKey { get; set; } = null!;


    [Required]
    public string SenderAccountNumber { get; set; } = null!;

    [Required]
    public string ReceiverAccountNumber { get; set; } = null!;

    [Column(TypeName = "decimal(15, 2)")]
    [Required]
    public decimal Amount { get; set; }

    [Required]
    public TransactionType Type { get; set; }

    [Required]
    public TransactionStatus Status { get; set; }

    public string? Description { get; set; }
    public string? FailureReason { get; set; }
    public string? ReferenceNumber { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }


}
public enum TransactionType
{
    TRANSFER,
    DEPOSIT,
    WITHDRAWAL,
    PAYMENT
}
public enum TransactionStatus
{
    PENDING,
    PROCESSING,
    PENDING_VERIFICATION,
    COMPLETED,
    FAILED,
    FLAGGED
}
