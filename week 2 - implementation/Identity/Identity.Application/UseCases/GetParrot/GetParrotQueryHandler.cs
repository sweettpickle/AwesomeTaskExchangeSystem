using Identity.Application.Utils.Common.Models;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace Identity.Application.UseCases.GetParrot
{
    public class GetParrotQueryHandler : IRequestHandler<GetParrotQuery, ParrotResult>
    {
        private readonly ISessionFactory _sessionFactory;

        public GetParrotQueryHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Task<ParrotResult> Handle(GetParrotQuery request, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            return session.Query<Domain.Parrot>()
                .Select(x => new ParrotResult
                {
                    PublicId = x.PublicId,
                    Nickname = x.Nickname,
                    Email = x.Email,
                    Address = x.Address,
                    Role = x.Role.ToString(),
                    PersonalAccount = new PersonalAccountInfo
                    {
                        AccountNumber = x.PersonalAccountInfo.AccountNumber,
                        AccountNickname = x.PersonalAccountInfo.AccountNickname
                    }
                })
                .FirstOrDefaultAsync(x => x.PublicId == request.PublicId, cancellationToken);
        }
    }
}
