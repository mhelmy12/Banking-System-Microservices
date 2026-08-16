using System;
using Confluent.Kafka;
namespace Account_Service.Consumers;

public class TransactionCompletedConsumer : BackgroundService
{
    private readonly ConsumerConfig _config;
    private readonly ILogger<TransactionCompletedConsumer> _logger;

    public TransactionCompletedConsumer(ILogger<TransactionCompletedConsumer> logger)
    {
        _logger = logger;
        _config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",

            GroupId = "account-service-group",

            AutoOffsetReset = AutoOffsetReset.Earliest
        };
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Task.Run(() => StartConsuming(stoppingToken), stoppingToken);
        return Task.CompletedTask;
    }

    private void StartConsuming(CancellationToken stoppingToken)
    {
        using var consumer = new ConsumerBuilder<string, object>(_config).Build();

        consumer.Subscribe("transaction.completed");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {

            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Kafka Consumer is stopping.");
        }
        finally
        {
            consumer.Close();
        }
    }


}
