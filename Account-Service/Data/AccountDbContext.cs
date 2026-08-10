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

      override protected void OnModelCreating(ModelBuilder modelBuilder)
      {
            base.OnModelCreating(modelBuilder);
            var entity = modelBuilder.Entity<Account>();


            entity.HasKey(a => a.Id);


            entity.Property(a => a.DailyTransactionLimit)
            .HasColumnType("decimal(18,2)");


            entity.Property(a => a.Balance)
            .HasColumnType("decimal(18,2)");



            entity.HasIndex(a => a.AccountNumber)
                  .IsUnique()
                  .HasDatabaseName("IX_Accounts_AccountNumber_Unique");

            entity.HasIndex(a => a.Email)
                  .IsUnique()
                  .HasDatabaseName("IX_Accounts_Email_Unique");
            entity.HasIndex(a => a.PhoneNumber)
                  .IsUnique()
                  .HasDatabaseName("IX_Accounts_PhoneNumber_Unique");


            entity.HasIndex(a => a.CreatedAt)
                  .HasDatabaseName("IX_Accounts_CreatedAt");

      }

}
