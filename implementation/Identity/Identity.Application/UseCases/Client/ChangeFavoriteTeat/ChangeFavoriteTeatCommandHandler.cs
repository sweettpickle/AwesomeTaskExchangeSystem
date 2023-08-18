using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace Identity.Application.UseCases.Client.ChangeFavoriteTeat
{
    internal class ChangeFavoriteTeatCommandHandler : IRequestHandler<ChangeFavoriteTeatCommand>
    {
        private readonly ISessionFactory _sessionFactory;

        public ChangeFavoriteTeatCommandHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task Handle(ChangeFavoriteTeatCommand request, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            var parrot = await session.Query<Domain.Parrot>()
                .FirstOrDefaultAsync(x => x.PublicId == request.PublicId, cancellationToken);

            if (parrot is null) return;

            var tran = session.BeginTransaction();
            parrot.ChangeFavoriteTreat(request.NewFavoriteTeat);
            await session.SaveOrUpdateAsync(parrot);
            await tran.CommitAsync();
        }
    }
}
