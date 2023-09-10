using Identity.Application.UseCases.Internal.OnParrotRoleChanged;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace Identity.Application.UseCases.Client.ChangeParrotRole
{
    internal class ChangeParrotRoleCommandHandler : IRequestHandler<ChangeParrotRoleCommand>
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly IMediator _mediator;

        public ChangeParrotRoleCommandHandler(ISessionFactory sessionFactory, IMediator mediator)
        {
            _sessionFactory = sessionFactory;
            _mediator = mediator;
        }

        public async Task Handle(ChangeParrotRoleCommand request, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            var parrot = await session.Query<Domain.Parrot>()
                .FirstOrDefaultAsync(x => x.PublicId == request.PublicId, cancellationToken);

            if (parrot is null) return;
            if (parrot.Role == request.NewRole) return;

            parrot.ChangeRole(request.NewRole);
            await session.SaveOrUpdateAsync(parrot);

            await _mediator.Publish(new ParrotRoleChangedEvent(parrot.PublicId, parrot.Role.ToString()));
        }
    }
}
