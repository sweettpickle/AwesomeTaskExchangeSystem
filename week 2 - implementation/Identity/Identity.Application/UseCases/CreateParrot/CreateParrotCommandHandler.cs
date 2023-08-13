using Identity.Application.Utils.Common.Models;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace Identity.Application.UseCases.CreateParrot
{
    internal class CreateParrotCommandHandler : IRequestHandler<CreateParrotCommand, ParrotResult>
    {
        private readonly ISessionFactory _sessionFactory;

        public CreateParrotCommandHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task<ParrotResult> Handle(CreateParrotCommand request, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            var parrot = await session.Query<Domain.Parrot>()
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (parrot != null) return null;

            parrot = new Domain.Parrot(
                request.Nickname, 
                request.Email, 
                request.Address, 
                request.Role,
                request.AccountNumber,
                request.AccountNickname,
                request.FavoriteTeat);

            using var tran = session.BeginTransaction();
            await session.SaveAsync(parrot);
            await tran.CommitAsync();

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
