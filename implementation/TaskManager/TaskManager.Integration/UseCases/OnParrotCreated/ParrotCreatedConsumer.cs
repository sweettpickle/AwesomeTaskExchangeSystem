using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TaskManager.Application.UseCases.Integration.CreateParrot;
using TaskManager.Integration.Utils.Kafka;

namespace TaskManager.Integration.UseCases.OnParrotCreated
{
    internal class ParrotCreatedConsumer : KafkaConsumer
    {
        private readonly IMediator _mediator;

        public ParrotCreatedConsumer(ILogger<KafkaConsumer> logger, KafkaConfig kafkaConfig, IMediator mediator)
            : base(logger, new ConsumerConfig
            {
                GroupId = "consumer_group_2",
                BootstrapServers = kafkaConfig.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
            }, kafkaConfig.ParrotsStreamingTopic)
        {
            _mediator = mediator;
        }

        protected override Task ProcessAsync(string message, CancellationToken cancellationToken)
        {
            var mes = JsonSerializer.Deserialize<MessageBase<object>>(message);
            if (mes.MessageType != "ParrotCreated") return Task.CompletedTask;

            var parrot = JsonSerializer.Deserialize<ParrotCreated>(mes.Data.ToString());
            
            return _mediator.Send(new CreateParrotCommand(parrot.PublicId, parrot.RolePid));
        }
    }
}
