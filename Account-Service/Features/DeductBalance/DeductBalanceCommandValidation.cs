using System;
using FluentValidation;

namespace Account_Service.Features.DeductBalance;

public class DeductBalanceCommandValidation : AbstractValidator<DeductBalanceCommand>
{
    public DeductBalanceCommandValidation()
    {
        RuleFor(x => x.AccountNumber)
            .NotEmpty()
            .WithMessage("Account number is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be a positive value");
    }

}
