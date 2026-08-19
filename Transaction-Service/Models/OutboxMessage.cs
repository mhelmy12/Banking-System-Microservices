using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transaction_Service.Models;

public class OutboxMessage
{

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    public string Id { get; set; }
    public string EventType { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public DateTime OccurredOn { get; set; }

    public string AggregateType { get; set; }   // Transaction
    public string AggregateId { get; set; }     // TransactionId
}


/**
At Up migration, we enable CDC for the OutboxMessages table to track changes for event sourcing.

        migrationBuilder.Sql(@"
            IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = DB_NAME() AND is_cdc_enabled = 1)
            BEGIN
                EXEC sys.sp_cdc_enable_db;
            END
        ");

        migrationBuilder.Sql(@"
            EXEC sys.sp_cdc_enable_table
            @source_schema = N'dbo',
            @source_name   = N'OutboxMessages',
            @role_name     = NULL,
            @supports_net_changes = 0;
        ");



At Down migration, we disable CDC for the OutboxMessages table.

        migrationBuilder.Sql(@"
            EXEC sys.sp_cdc_disable_table
            @source_schema = N'dbo',
            @source_name   = N'OutboxMessages';

**/
