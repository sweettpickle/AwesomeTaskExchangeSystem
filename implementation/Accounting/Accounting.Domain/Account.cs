using Accounting.Domain.Utils;

namespace Accounting.Domain
{
    public class Account : Entity
    {
        public virtual string AccountNumber { get; protected set; }
        public virtual decimal Balance { get; protected set; }

        public Account(Parrot parent, string accountNumber)
        {
            PublicId = parent.PublicId;
            Balance = 0;
            AccountNumber = accountNumber;
        }
    }
}
