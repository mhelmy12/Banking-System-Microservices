using System;

namespace Transaction_Service.Events;

public class TransactionInitiatedEvent
{
    public long TransactionId { get; set; }
    public string SenderAccountNumber { get; set; } = null!;
    public string ReceiverAccountNumber { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string ReferenceNumber { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

}
