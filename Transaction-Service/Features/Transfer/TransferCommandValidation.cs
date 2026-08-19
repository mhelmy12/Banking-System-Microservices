using System;
using FluentValidation;

namespace Transaction_Service.Features.Transfer;

public class TransferCommandValidation : AbstractValidator<TransferCommand>
{
    public TransferCommandValidation()
    {
        RuleFor(x => x.SenderAccountNumber)
            .NotEmpty().WithMessage("Sender account number is required.");

        RuleFor(x => x.ReceiverAccountNumber)
            .NotEmpty().WithMessage("Receiver account number is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Transfer amount must be greater than zero.");

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty().WithMessage("Idempotency key is required.");
    }
}

