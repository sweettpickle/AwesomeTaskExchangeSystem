using MediatR;

namespace Identity.Application.UseCases.Internal.OnParrotChanged
{
    public class ParrotChangedEvent : INotification
    {
        public string PublicId { get; }
        public string Nickname { get; }
        public string Email { get; }
        public string RolePid { get; }
        public string AccountNumber { get; }
        public string AccountNickname { get; }

        public ParrotChangedEvent(
            string publicId,
            string nickname,
            string email,
            string rolePid,
            string accountNumber,
            string accountNickname)
        {
            PublicId = publicId;
            Nickname = nickname;
            Email = email;
            RolePid = rolePid;
            AccountNumber = accountNumber;
            AccountNickname = accountNickname;
        }
    }
}
