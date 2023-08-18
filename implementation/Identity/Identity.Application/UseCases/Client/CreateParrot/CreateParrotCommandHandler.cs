using Identity.Application.UseCases.Internal.OnParrotCreated;
using Identity.Application.Utils.Common.Models;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace Identity.Application.UseCases.Client.CreateParrot
{
    internal class CreateParrotCommandHandler : IRequestHandler<CreateParrotCommand, ParrotResult>
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly IMediator _mediator;

        public CreateParrotCommandHandler(ISessionFactory sessionFactory, IMediator mediator)
        {
            _sessionFactory = sessionFactory;
            _mediator = mediator;
        }

        public async Task<ParrotResult> Handle(CreateParrotCommand request, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            var parrot = await session.Query<Domain.Parrot>()
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            //if (parrot != null) return null;

            parrot = new Domain.Parrot(
                request.Nickname,
                request.Email,
                request.Address,
                request.Role,
                request.AccountNumber,
                request.AccountNickname,
                request.FavoriteTeat);

            //await session.SaveAsync(parrot);

            await _mediator.Publish(new ParrotCreated(
                parrot.PublicId, 
                parrot.Nickname, 
                parrot.Email, 
                parrot.Role.ToString(),
                parrot.PersonalAccountInfo.AccountNumber,
                parrot.PersonalAccountInfo.AccountNickname));

            return new ParrotResult()
            {
                PublicId = parrot.PublicId,
                Nickname = parrot.Nickname,
                Email = parrot.Email,
                Address = parrot.Address,
                Role = parrot.Role.ToString(),
                PersonalAccount = new PersonalAccountInfo
                {
                    AccountNumber = parrot.PersonalAccountInfo.AccountNumber,
                    AccountNickname = parrot.PersonalAccountInfo.AccountNickname
                }
            };
        }
    }
}
