using Identity.Application.UseCases.Internal.OnParrotChanged;
using Identity.Application.UseCases.Internal.OnParrotRoleChanged;
using Identity.Integration.Utils.Kafka;
using Identity.Integration.Utils.Kafka.Models;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace Identity.Integration.UseCases.OnParrotUpdated
{
    internal class NotifyExternalSystemsOnParrotUpdated :
        INotificationHandler<ParrotChangedEvent>,
        INotificationHandler<ParrotRoleChangedEvent>
    {
        private readonly KafkaProducer<string, ParrotChangedData> _producer;
        private readonly ISessionFactory _sessionFactory;

        public NotifyExternalSystemsOnParrotUpdated(KafkaProducer<string, ParrotChangedData> producer)
        {
            _producer = producer;
        }

        public Task Handle(ParrotChangedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new MessageBase<ParrotChangedData>
            {
                MessageType = "ParrotChanged",
                Data = new ParrotChangedData
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

        public async Task Handle(ParrotRoleChangedEvent notification, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            var parrot = await session.Query<Domain.Parrot>()
                .FirstOrDefaultAsync(x => x.PublicId == notification.PublicId, cancellationToken);

            var @event = new MessageBase<ParrotChangedData>
            {
                MessageType = "ParrotChanged",
                Data = new ParrotChangedData
                {
                    PublicId = notification.PublicId,
                    Nickname = parrot.Nickname,
                    Email = parrot.Email,
                    RolePid = notification.RolePid,
                    PersonalAccountInfo = new PersonalAccountInfo
                    {
                        AccountNumber = parrot.PersonalAccountInfo.AccountNumber,
                        AccountNickname = parrot.PersonalAccountInfo.AccountNickname
                    }
                }
            };

            await _producer.SendMessageAsync("Parrots.Streaming", @event, cancellationToken);
        }
    }
}
