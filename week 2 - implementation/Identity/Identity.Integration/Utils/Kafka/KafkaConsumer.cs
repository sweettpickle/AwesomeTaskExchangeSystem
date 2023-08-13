using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace Identity.Integration.Utils.Kafka
{
    internal class KafkaConsumer : IHostedService
    {
        public readonly string Topic;
        
        public KafkaConsumer(string topic)
        {
            Topic = topic;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var conf = new ConsumerConfig
            {
                //GroupId = "st_consumer_group",
                BootstrapServers = "kafkaserver:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using (var builder = new ConsumerBuilder<Ignore,
                string>(conf).Build())
            {
                builder.Subscribe(Topic);
                var cancelToken = new CancellationTokenSource();
                try
                {
                    while (true)
                    {
                        var consumer = builder.Consume(cancelToken.Token);
                        Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");
                    }
                }
                catch (Exception)
                {
                    builder.Close();
                }
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
