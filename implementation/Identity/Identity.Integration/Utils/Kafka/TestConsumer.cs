using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Identity.Integration.Utils.Kafka
{
    internal class TestConsumer : KafkaConsumer
    {
        public TestConsumer(ILogger<KafkaConsumer> logger) 
            : base(logger, new ConsumerConfig
            {
                GroupId = "st_consumer_group",
                BootstrapServers = "kafkaserver:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            }, "Parrots.Streaming")
        {
        }

        protected override Task ProcessAsync(string message, CancellationToken cancellationToken)
        {
            Console.WriteLine("ooooooooooooooooooooo");
            return Task.CompletedTask;
        }
    }
}
