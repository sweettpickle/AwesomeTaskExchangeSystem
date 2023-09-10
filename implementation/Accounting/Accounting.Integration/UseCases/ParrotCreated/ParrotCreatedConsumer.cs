using Accounting.Application.UseCases.Integration.ParrotCreated;
using Accounting.Integration.Utils.Kafka;
using Accounting.Integration.Utils.Kafka.Models;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounting.Integration.UseCases.ParrotCreated
{
    internal class ParrotCreatedConsumer : KafkaBaseConsumer<Ignore, ParrotCreatedData>
    {
        private readonly IMediator _mediator;

        public ParrotCreatedConsumer(ILogger<KafkaBaseConsumer<Ignore, ParrotCreatedData>> logger, KafkaConfig kafkaConfig, IMediator mediator)
            : base(logger, new ConsumerConfig
            {
                GroupId = "Accounting",
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

            return _mediator.Publish(
                new ParrotCreatedInExternalSystem(parrot.PublicId, parrot.RolePid, parrot.Email, parrot.PersonalAccountInfo.AccountNumber, parrot.PersonalAccountInfo.AccountNickname),
                cancellationToken);
        }
    }
}
