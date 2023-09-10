using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.UseCases.Integration.ParrotCreated;
using TaskManager.Integration.Utils.Kafka;
using TaskManager.Integration.Utils.Kafka.Models;

namespace TaskManager.Integration.UseCases.ParrotCreated
{
    internal class ParrotCreatedConsumer : KafkaConsumer<Ignore, ParrotCreatedData>
    {
        private readonly IMediator _mediator;

        public ParrotCreatedConsumer(ILogger<KafkaConsumer<Ignore, ParrotCreatedData>> logger, KafkaConfig kafkaConfig, IMediator mediator)
            : base(logger, new ConsumerConfig
            {
                GroupId = "TaskManager",
                BootstrapServers = kafkaConfig.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
            }, kafkaConfig.ParrotsStreamingTopic)
        {
            _mediator = mediator;
        }

        protected override Task ProcessAsync(MessageBase<ParrotCreatedData> message, CancellationToken cancellationToken)
        {
            if (message.MessageType != "ParrotCreated") return Task.CompletedTask;

            var parrot = message.Data;

            return _mediator.Send(new CreateParrotCommand(parrot.PublicId, parrot.RolePid));
        }
    }
}
