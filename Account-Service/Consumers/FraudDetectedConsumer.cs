using System;
using Confluent.Kafka;

namespace Account_Service.Consumers;

public class FraudDetectedConsumer : BackgroundService
{
    private readonly ILogger<FraudDetectedConsumer> logger;
    private readonly ConsumerConfig _config;

    public FraudDetectedConsumer(ILogger<FraudDetectedConsumer> logger)
    {
        this.logger = logger;
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

        consumer.Subscribe("fraud.detected");

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
