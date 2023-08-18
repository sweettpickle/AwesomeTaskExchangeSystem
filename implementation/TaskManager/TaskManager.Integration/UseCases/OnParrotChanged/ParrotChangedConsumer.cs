using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TaskManager.Application.UseCases.Integration.OnParrotRoleChanged;
using TaskManager.Integration.Utils.Kafka;

namespace TaskManager.Integration.UseCases.OnParrotChanged
{
    internal class ParrotChangedConsumer : KafkaConsumer
    {
        private readonly IMediator _mediator;

        public ParrotChangedConsumer(ILogger<KafkaConsumer> logger, KafkaConfig kafkaConfig, IMediator mediator)
            : base(logger, new ConsumerConfig
            {
                GroupId = "consumer_group_1",
                BootstrapServers = kafkaConfig.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            }, kafkaConfig.ParrotsStreamingTopic)
        {
            _mediator = mediator;
        }

        protected override Task ProcessAsync(string message, CancellationToken cancellationToken)
        {
            var mes = JsonSerializer.Deserialize<MessageBase<object>>(message);
            if (mes.MessageType != "ParrotChanged") return Task.CompletedTask;

            if (mes.Data is ParrotChanged parrot == false)
                return Task.CompletedTask;

            return _mediator.Send(new ChangeParrotCommand(parrot.PublicId, parrot.RolePid));
        }
    }
}
