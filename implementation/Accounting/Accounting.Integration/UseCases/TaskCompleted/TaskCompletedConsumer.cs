using Accounting.Application.UseCases.Integration.TaskCompleted;
using Accounting.Integration.Utils.Kafka;
using Accounting.Integration.Utils.Kafka.Models;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounting.Integration.UseCases.TaskCompleted
{
    internal class TaskCompletedConsumer : KafkaBaseConsumer<Ignore, TaskCompletedData>
    {
        private readonly IMediator _mediator;

        public TaskCompletedConsumer(ILogger<KafkaBaseConsumer<Ignore, TaskCompletedData>> logger, KafkaConfig kafkaConfig, IMediator mediator)
            : base(logger, new ConsumerConfig
            {
                GroupId = "Accounting",
                BootstrapServers = kafkaConfig.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
            }, kafkaConfig.TasksBusinessEventsTopic)
        {
            _mediator = mediator;
        }

        protected override Task ProcessAsync(MessageBase<TaskCompletedData> message, CancellationToken cancellationToken)
        {
            if (message.MessageType != "TaskCompleted") return Task.CompletedTask;

            var data = message.Data;

            return _mediator.Publish(
                new TaskCompletedInExternalSystem(data.TaskPid, data.ParrotPid, data.Amount),
                cancellationToken);
        }
    }
}
