using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace TaskManager.Application.UseCases.Integration.OnParrotRoleChanged
{
    internal class ChangeParrotCommandHandler : IRequestHandler<ChangeParrotCommand>
    {
        private readonly ISessionFactory _sessionFactory;

        public ChangeParrotCommandHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task Handle(ChangeParrotCommand request, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            var parrot = await session.Query<Domain.Parrot>()
                .FirstOrDefaultAsync(x => x.PublicId == request.PublicId, cancellationToken);

            if (parrot is null) return;
            if (parrot.Role == request.Role) return;

            using var tran = session.BeginTransaction();
            parrot.ChangeRole(request.Role);
            await session.SaveOrUpdateAsync(parrot, cancellationToken);
            await tran.CommitAsync(cancellationToken);
        }
    }
}
