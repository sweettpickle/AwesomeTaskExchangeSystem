using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace Identity.Application.UseCases.ChangeParrotRole
{
    internal class ChangeParrotRoleCommandHandler : IRequestHandler<ChangeParrotRoleCommand>
    {
        private readonly ISessionFactory _sessionFactory;

        public ChangeParrotRoleCommandHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task Handle(ChangeParrotRoleCommand request, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            var parrot = await session.Query<Domain.Parrot>()
                .FirstOrDefaultAsync(x => x.PublicId == request.PublicId, cancellationToken);

            if (parrot is null) return;

            using var tran = session.BeginTransaction();
            parrot.ChangeRole(request.NewRole);
            await session.SaveOrUpdateAsync(parrot);
            await tran.CommitAsync();
        }
    }
}
