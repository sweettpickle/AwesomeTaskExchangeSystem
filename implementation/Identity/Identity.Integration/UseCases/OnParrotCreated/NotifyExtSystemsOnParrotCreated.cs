using MediatR;
using Identity.Integration.Utils.Kafka;
using Newtonsoft.Json.Linq;
using Identity.Application.UseCases.Internal.OnParrotCreated;

namespace Identity.Integration.UseCases.OnParrotCreated
{
    internal class NotifyExtSystemsOnParrotCreated : INotificationHandler<ParrotCreated>
    {
        private readonly KafkaProducer _producer;

        public NotifyExtSystemsOnParrotCreated(KafkaProducer producer)
        {
            _producer = producer;
        }

        public Task Handle(ParrotCreated notification, CancellationToken cancellationToken)
        {
            var @event = new
            {
                MessageType = "ParrotCreated",
                Data = new
                {
                    PublicId = notification.PublicId,
                    Nickname = notification.Nickname,
                    Email = notification.Email,
                    RolePid = notification.RolePid,
                    PersonalAccountInfo = new
                    {
                        AccountNumber = notification.AccountNumber,
                        AccountNickname = notification.AccountNickname
                    }
                }
            };

            return _producer.SendMessageAsync("Parrots.Streaming", JObject.FromObject(@event), cancellationToken);
        }
    }
}
