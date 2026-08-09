using System;
using Account_Service.Models;
using Microsoft.EntityFrameworkCore;
namespace Account_Service.Data;

public class AccountDbContext : DbContext
{

    public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }

}
