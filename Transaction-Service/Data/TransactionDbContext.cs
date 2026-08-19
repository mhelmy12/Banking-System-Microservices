using System;
using Microsoft.EntityFrameworkCore;
using Transaction_Service.Models;

namespace Transaction_Service.Data;

public class TransactionDbContext : DbContext
{
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }


        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
                base.OnModelCreating(modelBuilder);
                var entity = modelBuilder.Entity<Transaction>();

                entity.HasKey(t => t.Id);

                entity.Property(t => t.Amount)
                      .HasColumnType("decimal(15,2)");

                entity.HasIndex(t => t.SenderAccountNumber)
                        .HasDatabaseName("IX_Transactions_SenderAccountNumber");

                entity.HasIndex(t => t.ReceiverAccountNumber)
                        .HasDatabaseName("IX_Transactions_ReceiverAccountNumber");

                entity.HasIndex(t => t.CreatedAt)
                        .HasDatabaseName("IX_Transactions_CreatedAt");


                entity.HasIndex(t => t.ReferenceNumber)
                        .IsUnique()
                        .HasDatabaseName("IX_Transactions_ReferenceNumber_Unique");

                entity.Property(t => t.Type)
                      .HasConversion<string>()
                      .HasMaxLength(32);


                entity.Property(t => t.Status)
                    .HasConversion<string>()
                    .HasMaxLength(32);

                entity.Property(t => t.Description)
                      .HasMaxLength(256);

                entity.Property(t => t.FailureReason)
                      .HasMaxLength(256);



        }




}
