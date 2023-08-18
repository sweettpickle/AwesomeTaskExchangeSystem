using Identity.Application.Utils.Common.Models;
using MediatR;

namespace Identity.Application.UseCases.Client.Login
{
    public class LoginCommand : IRequest<ParrotResult>
    {
        public string Login { get; }
        public string FavoriteTreat { get; }

        public LoginCommand(string login, string favoriteTreat)
        {
            Login = login;
            FavoriteTreat = favoriteTreat;
        }
    }
}
