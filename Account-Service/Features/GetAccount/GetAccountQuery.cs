using System;
using System.ComponentModel.DataAnnotations;
using Account_Service.Helpers;
using Account_Service.Services.AccountNumberGenerator;
using MediatR;

namespace Account_Service.Features.GetAccount;

public class GetAccountQuery : IRequest<Response<GetAccountResponse>>
{


    [Required]
    public string AccountNumber { get; set; }

}
