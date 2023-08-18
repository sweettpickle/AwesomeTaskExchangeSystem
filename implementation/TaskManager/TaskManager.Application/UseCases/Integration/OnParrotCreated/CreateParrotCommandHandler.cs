using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace TaskManager.Application.UseCases.Integration.CreateParrot
{
    internal class CreateParrotCommandHandler : IRequestHandler<CreateParrotCommand>
    {
        private readonly ISessionFactory _sessionFactory;

        public CreateParrotCommandHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task Handle(CreateParrotCommand request, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            var parrot = await session.Query<Domain.Parrot>()
                .FirstOrDefaultAsync(x => x.PublicId == request.PublicId, cancellationToken);

            if (parrot != null) return;

            parrot = new Domain.Parrot(request.PublicId, request.Role);

            await session.SaveAsync(parrot);
        }
    }
}
