using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Identity.Integration.Utils.Kafka
{
    internal class KafkaProducer
    {
        private readonly ILogger<KafkaProducer> _logger;
        private readonly ProducerConfig config = new ProducerConfig { BootstrapServers = "kafkaserver:9092" };
        private readonly IProducer<Null, string> _producer;

        public KafkaProducer(ILogger<KafkaProducer> logger)
        {
            _logger = logger;
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task SendMessageAsync(string topic, string message)
        {
            _logger.LogInformation($"Producing message\n {message}\t to topic '{topic}'");

            try
            {
                await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                _logger.LogInformation($"Message\n {message}\t produced to topic '{topic}'");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Oops, something went wrong: {e}");
            }
        }
    }
}
