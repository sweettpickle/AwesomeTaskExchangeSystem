using Identity.Application.Utils.Common.Models;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace Identity.Application.UseCases.GetParrots
{
    internal class GetParrotsQueryHandler : IRequestHandler<GetParrotsQuery, List<ParrotResult>>
    {
        private readonly ISessionFactory _sessionFactory;

        public GetParrotsQueryHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task<List<ParrotResult>> Handle(GetParrotsQuery request, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            return await session.Query<Domain.Parrot>()
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
                .ToListAsync(cancellationToken);
        }
    }
}
