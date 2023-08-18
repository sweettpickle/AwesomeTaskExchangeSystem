using Identity.Application.UseCases.Internal.OnParrotRoleChanged;
using Identity.Integration.Utils.Kafka;
using MediatR;
using Newtonsoft.Json.Linq;

namespace Identity.Integration.UseCases.OnParrotRoleChanged
{
    internal class NotifyExtSystemsOnParrotRoleChanged : INotificationHandler<ParrotRoleChanged>
    {
        private readonly KafkaProducer _producer;

        public NotifyExtSystemsOnParrotRoleChanged(KafkaProducer producer)
        {
            _producer = producer;
        }

        public Task Handle(ParrotRoleChanged notification, CancellationToken cancellationToken)
        {
            var @event = new
            {
                MessageType = "ParrotRoleChanged",
                Data = new
                {
                    PublicId = notification.PublicId,
                    RolePid = notification.RolePid,
                }
            };

            return _producer.SendMessageAsync("Parrots.Events", JObject.FromObject(@event), cancellationToken);
        }
    }
}
