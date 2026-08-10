using System;
using FluentValidation;

namespace Account_Service.Features.CreateAccount;

public class CreateAccountCommandValidation : AbstractValidator<CreateAccountCommand>
{

    public CreateAccountCommandValidation()
    {
        ApplyValidators();

    }

    public void ApplyValidators()
    {
        RuleFor(c => c.AccountHolderName)
            .NotEmpty().WithMessage("Account holder name is required.")
            .MaximumLength(100).WithMessage("Account holder name cannot exceed 100 characters.");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(c => c.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

        RuleFor(c => c.AccountType)
            .NotEmpty().WithMessage("Account type is required.")
            .Must(type => type == "Savings" || type == "Checking" || type == "Credit").WithMessage("Account type must be either 'Savings', 'Checking', or 'Credit'.");

        RuleFor(c => c.InitialDeposit)
            .GreaterThanOrEqualTo(0).WithMessage("Initial deposit must be a non-negative value.");
    }

}
