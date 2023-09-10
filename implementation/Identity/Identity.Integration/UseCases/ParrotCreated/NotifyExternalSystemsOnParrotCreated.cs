using MediatR;
using Identity.Integration.Utils.Kafka;
using Identity.Application.UseCases.Internal.OnParrotCreated;
using Identity.Integration.Utils.Kafka.Models;

namespace Identity.Integration.UseCases.OnParrotCreated
{
    internal class NotifyExternalSystemsOnParrotCreated : INotificationHandler<ParrotCreated>
    {
        private readonly KafkaProducer<string, ParrotCreatedData> _producer;

        public NotifyExternalSystemsOnParrotCreated(KafkaProducer<string, ParrotCreatedData> producer)
        {
            _producer = producer;
        }

        public Task Handle(ParrotCreated notification, CancellationToken cancellationToken)
        {
            var @event = new MessageBase<ParrotCreatedData>
            {
                MessageType = "ParrotCreated",
                Data = new ParrotCreatedData
                {
                    PublicId = notification.PublicId,
                    Nickname = notification.Nickname,
                    Email = notification.Email,
                    RolePid = notification.RolePid,
                    PersonalAccountInfo = new PersonalAccountInfo
                    {
                        AccountNumber = notification.AccountNumber,
                        AccountNickname = notification.AccountNickname
                    }
                }
            };

            return _producer.SendMessageAsync("Parrots.Streaming", @event, cancellationToken);
        }
    }
}
