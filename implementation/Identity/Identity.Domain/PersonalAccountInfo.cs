using Identity.Domain.Utils;

namespace Identity.Domain
{
    public class PersonalAccountInfo : Entity
    {
        public virtual Parrot Parent { get; protected set; }
        public virtual string AccountNumber { get; protected set; }
        public virtual string AccountNickname { get; protected set; }

        protected PersonalAccountInfo() { }

        public PersonalAccountInfo(Parrot parent, string accountNumber, string accountNickname)
        {
            Parent = parent;
            PublicId = parent.PublicId;
            AccountNumber = accountNumber;
            AccountNickname = accountNickname;
        }
    }
}
