using MediatR;

namespace Accounting.Application.UseCases.Integration.ParrotCreated
{
    public class ParrotCreatedInExternalSystem : INotification
    {
        public string PublicId { get; }
        public string RolePid { get; }
        public string Email { get; }
        public string AccountNumber { get; }
        public string AccountNickname { get; }

        public ParrotCreatedInExternalSystem(string publicId, string rolePid, string email, string accountNumber, string accountNickname)
        {
            PublicId = publicId;
            RolePid = rolePid;
            Email = email;
            AccountNumber = accountNumber;
            AccountNickname = accountNickname;
        }
    }
}
