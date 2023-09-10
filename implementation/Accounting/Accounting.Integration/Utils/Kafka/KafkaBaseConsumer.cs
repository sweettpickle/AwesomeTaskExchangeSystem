using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Accounting.Integration.Utils.Kafka
{
    internal abstract class KafkaBaseConsumer<K, V> : IHostedService
    {
        protected readonly ILogger<KafkaBaseConsumer<K, V>> Logger;
        protected readonly ConsumerConfig ConsumerConfig;
        protected readonly string Topic;

        protected IConsumer<K, MessageBase<V>> Consumer;

        public KafkaBaseConsumer(ILogger<KafkaBaseConsumer<K, V>> logger, 
            ConsumerConfig consumerConfig, string topic)
        {
            Logger = logger;
            ConsumerConfig = consumerConfig;
            Topic = topic;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Consumer = new ConsumerBuilder<K, MessageBase<V>>(ConsumerConfig)
                //.SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(new JsonDeserializer<MessageBase<V>>().AsSyncOverAsync())
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .Build();

            Consumer.Subscribe(Topic);

            Task.Run(() =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    try
                    {
                        var consumer = Consumer.Consume(cancellationToken);
                        if (consumer.Message == null) continue;

                        Logger.LogInformation($"Message: {System.Text.Json.JsonSerializer.Serialize(consumer.Message.Value)} received from {consumer.TopicPartitionOffset}");
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

        protected abstract Task ProcessAsync(MessageBase<V> message, CancellationToken cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Consumer.Close();
            Consumer.Dispose();
            return Task.CompletedTask;
        }
    }
}
