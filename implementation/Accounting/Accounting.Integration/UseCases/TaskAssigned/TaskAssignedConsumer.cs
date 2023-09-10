using Accounting.Application.UseCases.Integration.TaskAssigned;
using Accounting.Integration.Utils.Kafka;
using Accounting.Integration.Utils.Kafka.Models;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounting.Integration.UseCases.TaskAssigned
{
    internal class TaskAssignedConsumer : KafkaBaseConsumer<Ignore, TaskAssignedData>
    {
        private readonly IMediator _mediator;

        public TaskAssignedConsumer(ILogger<KafkaBaseConsumer<Ignore, TaskAssignedData>> logger, KafkaConfig kafkaConfig, IMediator mediator)
            : base(logger, new ConsumerConfig
            {
                GroupId = "Accounting",
                BootstrapServers = kafkaConfig.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
            }, kafkaConfig.TasksBusinessEventsTopic)
        {
            _mediator = mediator;
        }

        protected override Task ProcessAsync(MessageBase<TaskAssignedData> message, CancellationToken cancellationToken)
        {
            if (message.MessageType != "TaskAssigned") return Task.CompletedTask;

            var data = message.Data;

            return _mediator.Publish(
                new TaskAssignedInExternalSystem(data.TaskPid, data.ParrotPid, data.Amount),
                cancellationToken);
        }
    }
}
