using Identity.Application.UseCases.Internal.OnParrotChanged;
using Identity.Application.UseCases.Internal.OnParrotRoleChanged;
using Identity.Integration.Utils.Kafka;
using MediatR;
using Newtonsoft.Json.Linq;
using NHibernate;
using NHibernate.Linq;

namespace Identity.Integration.UseCases.OnParrotUpdated
{
    internal class NotifyExtSystemsOnParrotUpdated :
        INotificationHandler<ParrotChanged>,
        INotificationHandler<ParrotRoleChanged>
    {
        private readonly KafkaProducer _producer;
        private readonly ISessionFactory _sessionFactory;

        public NotifyExtSystemsOnParrotUpdated(KafkaProducer producer)
        {
            _producer = producer;
        }

        public Task Handle(ParrotChanged notification, CancellationToken cancellationToken)
        {
            var @event = new
            {
                MessageType = "ParrotChanged",
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

        public async Task Handle(ParrotRoleChanged notification, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            var parrot = await session.Query<Domain.Parrot>()
                .FirstOrDefaultAsync(x => x.PublicId == notification.PublicId, cancellationToken);

            var @event = new
            {
                MessageType = "ParrotChanged",
                Data = new
                {
                    PublicId = notification.PublicId,
                    Nickname = parrot.Nickname,
                    Email = parrot.Email,
                    RolePid = notification.RolePid,
                    PersonalAccountInfo = new
                    {
                        AccountNumber = parrot.PersonalAccountInfo.AccountNumber,
                        AccountNickname = parrot.PersonalAccountInfo.AccountNickname
                    }
                }
            };

            await _producer.SendMessageAsync("Parrots.Streaming", JObject.FromObject(@event), cancellationToken);
        }
    }
}
