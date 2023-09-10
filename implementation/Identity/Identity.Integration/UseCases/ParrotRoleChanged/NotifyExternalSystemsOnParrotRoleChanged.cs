using Identity.Application.UseCases.Internal.OnParrotRoleChanged;
using Identity.Integration.Utils.Kafka;
using Identity.Integration.Utils.Kafka.Models;
using MediatR;

namespace Identity.Integration.UseCases.OnParrotRoleChanged
{
    internal class NotifyExternalSystemsOnParrotRoleChanged : INotificationHandler<ParrotRoleChangedEvent>
    {
        private readonly KafkaProducer<string, ParrotRoleChangedData> _producer;

        public NotifyExternalSystemsOnParrotRoleChanged(KafkaProducer<string, ParrotRoleChangedData> producer)
        {
            _producer = producer;
        }

        public Task Handle(ParrotRoleChangedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new MessageBase<ParrotRoleChangedData>
            {
                MessageType = "ParrotRoleChanged",
                Data = new ParrotRoleChangedData
                {
                    PublicId = notification.PublicId,
                    RolePid = notification.RolePid,
                }
            };

            return _producer.SendMessageAsync("Parrots.Events", @event, cancellationToken);
        }
    }
}
