using Accounting.Domain;
using FluentNHibernate.Mapping;

namespace Accounting.Persistence.Utils.Mappings
{
    internal class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            Id(e => e.Id).GeneratedBy.Identity();
            Map(e => e.CreatedAt).Generated.Insert();
            Map(e => e.PublicId).Not.Nullable().Length(255);

            Map(e => e.AccountNumber).Not.Nullable().Length(255);
            Map(e => e.Balance).Not.Nullable();

            Table("Account");
        }
    }
}
