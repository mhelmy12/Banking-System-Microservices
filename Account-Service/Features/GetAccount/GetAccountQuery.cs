using System;
using System.ComponentModel.DataAnnotations;
using Shared.Helpers;
using MediatR;

namespace Account_Service.Features.GetAccount;

public class GetAccountQuery : IRequest<Response<GetAccountResponse>>
{


    [Required]
    public string AccountNumber { get; set; }

}
