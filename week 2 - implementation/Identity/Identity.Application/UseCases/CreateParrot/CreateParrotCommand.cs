using Identity.Application.Utils.Common.Models;
using MediatR;

namespace Identity.Application.UseCases.CreateParrot
{
    public class CreateParrotCommand : IRequest<ParrotResult>
    {
        public string Nickname { get; }
        public string Email { get; }
        public string Address { get; }
        public RoleEnum Role { get; set; }
        public string AccountNumber { get; }
        public string AccountNickname { get; }
        public string FavoriteTeat { get; }

        public CreateParrotCommand(
            string nickname, 
            string email, 
            string address, 
            RoleEnum role, 
            string accountNumber, 
            string accountNickname,
            string favoriteTrat)
        {
            Nickname = nickname;
            Email = email;
            Address = address;
            Role = role;
            AccountNumber = accountNumber;
            AccountNickname = accountNickname;
            FavoriteTeat = favoriteTrat;
        }
    }
}
