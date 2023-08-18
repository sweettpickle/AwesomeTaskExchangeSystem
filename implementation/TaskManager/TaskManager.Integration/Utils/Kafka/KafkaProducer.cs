using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace TaskManager.Integration.Utils.Kafka
{
    internal class KafkaProducer
    {
        private readonly ILogger<KafkaProducer> _logger;
        private readonly ProducerConfig _config;
        private readonly IProducer<Null, string> _producer;

        public KafkaProducer(ILogger<KafkaProducer> logger, KafkaConfig kafkaConfig)
        {
            _logger = logger;
            _config = new ProducerConfig { BootstrapServers = kafkaConfig.BootstrapServers };
            _producer = new ProducerBuilder<Null, string>(_config).Build();
        }

        public async Task SendMessageAsync(string topic, JObject message, CancellationToken token = default)
        {
            //_logger.LogInformation($"Producing message\n {message} to topic '{topic}'");

            try
            {
                await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message.ToString() }, token);

                _logger.LogInformation($"Message\n {message}\t produced to topic '{topic}'");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Oops, something went wrong");
            }
        }

        public async Task SendMessageAsync(string topic, string message, CancellationToken token = default)
        {
            _logger.LogInformation($"Producing message\n {message}\t to topic '{topic}'");

            try
            {
                await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message }, token);

                _logger.LogInformation($"Message\n {message}\t produced to topic '{topic}'");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Oops, something went wrong");
            }
        }
    }
}
