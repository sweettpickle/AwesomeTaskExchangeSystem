using Identity.Integration.Utils.Kafka;
using MediatR;
using System.Text.Json;

namespace Identity.Integration.UseCases.SendMessageToExternalSystems
{
    internal class SendMessageCommandHandler : IRequestHandler<SendMessageCommand>
    {
        private readonly KafkaProducer _producer;

        public SendMessageCommandHandler(KafkaProducer producer)
        {
            _producer = producer;
        }

        public Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var message = JsonSerializer.Serialize(request.Message);
            if (string.IsNullOrEmpty(message)) return Task.CompletedTask;

            return _producer.SendMessageAsync(request.Topic, message);
        }
    }
}
