using Identity.Application.Utils.Common.Models;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace Identity.Application.UseCases.RemoveParrot
{
    internal class RemoveParrotCommandHandler : IRequestHandler<RemoveParrotCommand>
    {
        private readonly ISessionFactory _sessionFactory;

        public RemoveParrotCommandHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task Handle(RemoveParrotCommand request, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            var parrot = await session.Query<ParrotResult>()
                .FirstOrDefaultAsync(x => x.PublicId == request.PublicId, cancellationToken);
            if (parrot is null) return;

            using var tran = session.BeginTransaction();
            await session.DeleteAsync(parrot);
            await tran.CommitAsync();
        }
    }
}
