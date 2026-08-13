using System;
using Account_Service.Helpers;
using FluentValidation;
using Microsoft.Identity.Client;

namespace Account_Service.Features.FreezeAccount;

public class FreezeAccountValidation : AbstractValidator<FreezeAccountCommand>
{
    public FreezeAccountValidation()
    {
        RuleFor(x => x.AccountNumber)
            .Must(x => LuhnAlgorithm.IsValid(x))
            .WithMessage("Invalid account number.")
            .NotEmpty()
            .WithMessage("Account number is required.");

    }

}
