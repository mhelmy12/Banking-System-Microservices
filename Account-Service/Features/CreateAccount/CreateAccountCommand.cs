using System;
using Account_Service.Helpers;
using MediatR;

namespace Account_Service.Features.CreateAccount;

public class CreateAccountCommand : IRequest<CreateAccountResponse> , ITransactionCommand
{

    public string AccountHolderName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string AccountType { get; set; }
    public decimal InitialDeposit { get; set; }
}
