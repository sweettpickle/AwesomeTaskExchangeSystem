using Accounting.Domain.Utils;
using Newtonsoft.Json.Linq;

namespace Accounting.Domain
{
    public class Transaction : Entity
    {
        public virtual Account Account { get; protected set; }
        public virtual TransactionTypeEnum Type { get; protected set; }
        public virtual decimal Amount { get; protected set; }
        public virtual JToken ExtraData { get; protected set; }

        public Transaction(Account account, TransactionTypeEnum type, decimal amount, JToken extraData)
        {
            if (Enum.IsDefined(typeof(TransactionTypeEnum), type) == false)
                throw new ArgumentException("Argument not valid", nameof(Type));

            if (amount == 0)
                throw new ArgumentException("Amount cant be null", nameof(Amount));

            PublicId = Guid.NewGuid().ToString();
            Account = account;
            Type = type;
            Amount = amount;
            ExtraData = extraData;
        }
    }
}
