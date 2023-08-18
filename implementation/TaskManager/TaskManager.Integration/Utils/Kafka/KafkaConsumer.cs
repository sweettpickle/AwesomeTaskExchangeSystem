using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TaskManager.Integration.Utils.Kafka
{
    internal abstract class KafkaConsumer : IHostedService
    {
        protected readonly ILogger<KafkaConsumer> Logger;
        protected readonly ConsumerConfig ConsumerConfig;
        protected readonly string Topic;

        protected IConsumer<Ignore, string> Consumer;

        public KafkaConsumer(ILogger<KafkaConsumer> logger, ConsumerConfig consumerConfig, string topic)
        {
            Logger = logger;
            ConsumerConfig = consumerConfig;
            Topic = topic;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Consumer = new ConsumerBuilder<Ignore, string>(ConsumerConfig).Build();
            Consumer.Subscribe(Topic);

            Task.Run(() =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    var consumer = Consumer.Consume(cancellationToken);

                    if (consumer.Message == null) continue;

                    try
                    {
                        Logger.LogInformation($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");
                        ProcessAsync(consumer.Message.Value, cancellationToken);
                    }
                    catch (Exception e)
                    {
                        Logger.LogError(e, $"Oops, something went wrong while process message");
                    }
                }
            });

            Logger.LogInformation("Consumer successfull subscribed to topic {Topic}", Topic);
            return Task.CompletedTask;
        }

        protected abstract Task ProcessAsync(string message, CancellationToken cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Consumer.Close();
            return Task.CompletedTask;
        }
    }
}
