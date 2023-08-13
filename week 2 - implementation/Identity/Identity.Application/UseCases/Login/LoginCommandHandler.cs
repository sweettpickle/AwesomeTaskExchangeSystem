using Identity.Application.Utils.Common.Models;
using Identity.Shared;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace Identity.Application.UseCases.Login
{
    internal class LoginCommandHandler : IRequestHandler<LoginCommand, ParrotResult>
    {
        private readonly ISessionFactory _sessionFactory;

        public LoginCommandHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task<ParrotResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            var authInfo = await session.Query<Domain.AuthInfo>()
                .FirstOrDefaultAsync(x => x.Login == request.Login);

            if (authInfo is null)
                return null;

            byte[] hashBytes = authInfo.FavoriteTreatHash;
            PasswordHash hash = new PasswordHash(hashBytes);
            
            if (!hash.Verify(request.FavoriteTreat))
                return null;

            return new ParrotResult
            {
                PublicId = authInfo.Parent.PublicId,
                Nickname = authInfo.Parent.Nickname,
                Email = authInfo.Parent.Email,
                Address = authInfo.Parent.Address,
                Role = authInfo.Parent.Role.ToString(),
                PersonalAccount = new PersonalAccountInfo
                {
                    AccountNumber = authInfo.Parent.PersonalAccountInfo.AccountNumber,
                    AccountNickname = authInfo.Parent.PersonalAccountInfo.AccountNickname
                }
            };
        }
    }
}
