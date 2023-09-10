using Accounting.Domain;
using FluentNHibernate.Mapping;
using NHibernate.Type;

namespace Accounting.Persistence.Utils.Mappings
{
    internal class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            Id(e => e.Id).GeneratedBy.Identity();
            Map(e => e.CreatedAt).Generated.Insert();
            Map(e => e.PublicId).Not.Nullable().Length(255);

            Map(e => e.Type).CustomType<EnumStringType<TransactionTypeEnum>>();
            Map(e => e.Account).Not.Nullable();
            Map(e => e.Amount).Not.Nullable();
            Map(e => e.ExtraData).Nullable();

            Table("Transaction");
        }
    }
}
