using Accounting.Domain.Utils;

namespace Accounting.Domain
{
    public class Parrot : Entity
    {
        public virtual RoleEnum Role { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual Account Account { get; protected set; }

        public Parrot(string publicId, RoleEnum role, string accountNumber, string email)
        {
            PublicId = publicId;
            Role = role;
            Account = new Account(this, accountNumber);
            Email = email;
        }
    }
}
