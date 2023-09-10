using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.UseCases.Integration.ParrotRoleChanged;
using TaskManager.Integration.Utils.Kafka;
using TaskManager.Integration.Utils.Kafka.Models;

namespace TaskManager.Integration.UseCases.ParrotChanged
{
    internal class ParrotChangedConsumer : KafkaConsumer<Ignore, ParrotChangedData>
    {
        private readonly IMediator _mediator;

        public ParrotChangedConsumer(ILogger<KafkaConsumer<Ignore, ParrotChangedData>> logger, KafkaConfig kafkaConfig, IMediator mediator)
            : base(logger, new ConsumerConfig
            {
                GroupId = "TaskManager",
                BootstrapServers = kafkaConfig.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            }, kafkaConfig.ParrotsStreamingTopic)
        {
            _mediator = mediator;
        }

        protected override Task ProcessAsync(MessageBase<ParrotChangedData> message, CancellationToken cancellationToken)
        {
            if (message.MessageType != "ParrotChanged") return Task.CompletedTask;

            var parrot = message.Data;

            return _mediator.Send(new ChangeParrotCommand(parrot.PublicId, parrot.RolePid));
        }
    }
}
