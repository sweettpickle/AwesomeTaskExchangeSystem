using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Logging;

namespace Identity.Integration.Utils.Kafka
{
    internal class KafkaProducer<K, V>
    {
        private readonly ILogger<KafkaProducer<K, MessageBase<V>>> _logger;
        private readonly IProducer<K, MessageBase<V>> _producer;
        private readonly CachedSchemaRegistryClient _schemaRegistry;

        public KafkaProducer(ILogger<KafkaProducer<K, MessageBase<V>>> logger, KafkaConfig kafkaConfig)
        {
            _logger = logger;
            
            var config = new ProducerConfig { BootstrapServers = kafkaConfig.BootstrapServers };
            var schemaRegistryConfig = new SchemaRegistryConfig { Url = kafkaConfig.SchemaRegistryUrl.ToString() };
            
            _schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
            _producer = new ProducerBuilder<K, MessageBase<V>>(config)
                .SetValueSerializer(new JsonSerializer<MessageBase<V>>(_schemaRegistry))
                .Build();
        }

        public async Task SendMessageAsync(string topic, MessageBase<V> data, CancellationToken token = default)
        {
            try
            {
                var message = new Message<K, MessageBase<V>> { Value = data };
                await _producer.ProduceAsync(topic, message, token);

                _logger.LogInformation($"Message\n {System.Text.Json.JsonSerializer.Serialize(data)}\t produced to topic '{topic}'");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Oops, something went wrong");
            }
        }

        //public async Task SendMessageAsync(string topic, string message, CancellationToken token = default)
        //{
        //    _logger.LogInformation($"Producing message\n {message}\t to topic '{topic}'");

        //    try
        //    {
        //        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message }, token);

        //        _logger.LogInformation($"Message\n {message}\t produced to topic '{topic}'");
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e, "Oops, something went wrong");
        //    }
        //}
    }
}
